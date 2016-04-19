﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.ImageProviders;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using TestPlugin.Annotations;

namespace TestPlugin.NiceLobby
{
	[Plugin(PluginName = "NiceLobby", Description = "", PluginVersion = "1.0", Author = "MiNET Team"), UsedImplicitly]
	public class NiceLobbyPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (NiceLobbyPlugin));

		[UsedImplicitly] private Timer _popupTimer;
		[UsedImplicitly] private Timer _tickTimer;

		private long _tick = 0;

		protected override void OnEnable()
		{
			var server = Context.Server;

			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;
				level.BlockBreak += LevelOnBlockBreak;
				level.BlockPlace += LevelOnBlockPlace;
			};

			server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				Player player = args.Player;
				player.PlayerJoin += OnPlayerJoin;
				player.PlayerLeave += OnPlayerLeave;
			};

			_popupTimer = new Timer(DoDevelopmentPopups, null, 10000, 30000);
			//_tickTimer = new Timer(LevelTick, null, 0, 50);
		}

		private void OnPlayerLeave(object o, PlayerEventArgs eventArgs)
		{
			Level level = eventArgs.Level;
			if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

			Player player = eventArgs.Player;
			if (player == null) throw new ArgumentNullException(nameof(eventArgs.Player));

			level.BroadcastMessage($"{ChatColors.Gold}[{ChatColors.Red}-{ChatColors.Gold}]{ChatFormatting.Reset} {player.Username}");
		}

		private void OnPlayerJoin(object o, PlayerEventArgs eventArgs)
		{
			Level level = eventArgs.Level;
			if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

			Player player = eventArgs.Player;
			if(player == null) throw new ArgumentNullException(nameof(eventArgs.Player));

			level.BroadcastMessage($"{ChatColors.Gold}[{ChatColors.Green}+{ChatColors.Gold}]{ChatFormatting.Reset} {player.Username}");
		}

		private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
		{
			if (e.Block.Coordinates.DistanceTo((BlockCoordinates) e.Player.SpawnPosition) < 15)
			{
				e.Cancel = e.Player.GameMode != GameMode.Creative;
			}
		}

		private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
		{
			if (e.ExistingBlock.Coordinates.DistanceTo((BlockCoordinates) e.Player.SpawnPosition) < 15)
			{
				e.Cancel = e.Player.GameMode != GameMode.Creative;
			}
		}

		private double m = 0.1d;

		private void LevelTick(object state)
		{
			if (m > 0)
			{
				//if (_tick%random.Next(1, 4) == 0)
				Level level = Context.LevelManager.Levels.FirstOrDefault();
				if (level == null) return;

				Random random = level.Random;

				PlayerLocation point1 = level.SpawnPoint;
				PlayerLocation point2 = level.SpawnPoint;
				point2.X += 10;
				PlayerLocation point3 = level.SpawnPoint;
				point3.X -= 10;

				if (Math.Abs(m - 3) < 0.1)
				{
					McpeSetTime timeDay = McpeSetTime.CreateObject();
					timeDay.time = 0;
					timeDay.started = 1;
					level.RelayBroadcast(timeDay, true);

					ThreadPool.QueueUserWorkItem(delegate(object o)
					{
						Thread.Sleep(100);

						McpeSetTime timeReset = McpeSetTime.CreateObject();
						timeReset.time = (int) level.CurrentWorldTime;
						timeReset.started = (byte) (level.IsWorldTimeStarted ? 1 : 0);
						level.RelayBroadcast(timeDay, true);

						Thread.Sleep(200);

						{
							var mcpeExplode = McpeExplode.CreateObject();
							mcpeExplode.x = point1.X;
							mcpeExplode.y = point1.Y;
							mcpeExplode.z = point1.Z;
							mcpeExplode.radius = 100;
							mcpeExplode.records = new Records();
							level.RelayBroadcast(mcpeExplode, true);
						}

						Thread.Sleep(250);
						{
							var mcpeExplode = McpeExplode.CreateObject();
							mcpeExplode.x = point2.X;
							mcpeExplode.y = point2.Y;
							mcpeExplode.z = point2.Z;
							mcpeExplode.radius = 100;
							mcpeExplode.records = new Records();
							level.RelayBroadcast(mcpeExplode, true);
						}
						Thread.Sleep(250);
						{
							var mcpeExplode = McpeExplode.CreateObject();
							mcpeExplode.x = point3.X;
							mcpeExplode.y = point3.Y;
							mcpeExplode.z = point3.Z;
							mcpeExplode.radius = 100;
							mcpeExplode.records = new Records();
							level.RelayBroadcast(mcpeExplode, true);
						}
					});
				}

				if (m < 0.4 || m > 3)
					for (int i = 0; i < 15 + (30*m); i++)
					{
						GenerateParticles(random, level, point1, m < 0.6 ? 0 : 20, new Vector3(m*(m/2), m + 10, m*(m/2)), m);
						GenerateParticles(random, level, point2, m < 0.4 ? 0 : 12, new Vector3(m, m + 6, m), m);
						GenerateParticles(random, level, point3, m < 0.2 ? 0 : 9, new Vector3(m/2, m/2 + 6, m/2), m);
					}
			}
			m += 0.1;
			if (m > 3.8) m = -5;
		}

		private void GenerateParticles(Random random, Level level, PlayerLocation point, float yoffset, Vector3 multiplier, double d)
		{
			float vx = (float) random.NextDouble();
			vx *= random.Next(2) == 0 ? 1 : -1;
			vx *= (float) multiplier.X;

			float vy = (float) random.NextDouble();
			//vy *= random.Next(2) == 0 ? 1 : -1;
			vy *= (float) multiplier.Y;

			float vz = (float) random.NextDouble();
			vz *= random.Next(2) == 0 ? 1 : -1;
			vz *= (float) multiplier.Z;

			McpeLevelEvent mobParticles = McpeLevelEvent.CreateObject();
			mobParticles.eventId = (short) (0x4000 | GetParticle(random.Next(0, m < 1 ? 2 : 5)));
			mobParticles.x = point.X + vx;
			mobParticles.y = (point.Y - 2) + yoffset + vy;
			mobParticles.z = point.Z + vz;
			level.RelayBroadcast(mobParticles);
		}

		private short GetParticle(int rand)
		{
			switch (rand)
			{
				case 0:
					return (short) ParticleType.Explode; // Expload
					break;
				case 1:
					return (short) ParticleType.Flame; // Flame
					break;
				case 2:
					return (short) ParticleType.Lava; // Lava
					break;
				case 3:
					return (short) ParticleType.Critical; // Critical
					break;
				case 4:
					return (short) ParticleType.DripLava; // Lava drip
					break;
				case 5:
					return (short) ParticleType.MobFlame; // Entity flame
					break;
			}

			return 4;
		}

		//[PacketHandler, Receive, UsedImplicitly]
		//public Package ChatHandler(McpeText text, Player player)
		//{
		//	if (text.message.StartsWith("/") || text.message.StartsWith(".")) return text;

		//	player.Level.BroadcastTextMessage((" §7" + player.Username + "§7: §r§f" + text.message), null, MessageType.Raw);
		//	return null;
		//}

		[PacketHandler, Receive, UsedImplicitly]
		public Package LoginHandler(McpeLogin packet, Player player)
		{
			player.DisplayName = TextUtils.Center($"{GetNameTag(packet.username ?? "")}");
			return packet;
		}

		[PacketHandler, Send, UsedImplicitly]
		public Package RespawnHandler(McpeRespawn packet, Player player)
		{
			SendNameTag(player);
			player.RemoveAllEffects();

			player.SetEffect(new Speed {Level = 1, Duration = Effect.MaxDuration}); // 10s in ticks
			//player.SetEffect(new Slowness { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new Haste { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new MiningFatigue { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new Strength { Level = 20, Duration = 20 * 10 });
			player.SetEffect(new JumpBoost {Level = 1, Duration = Effect.MaxDuration});
			//player.SetEffect(new Blindness { Level = 20, Duration = 20 * 10 });
			//player.SetAutoJump(true);

			if (player.Level.LevelId.Equals("Default"))
			{
				player.Level.CurrentWorldTime = 6000;
				player.Level.IsWorldTimeStarted = false;
			}

			player.SendSetTime();

			return packet;
		}

		[PacketHandler, Send, UsedImplicitly]
		public Package AddPlayerHandler(McpeAddPlayer packet, Player player)
		{
			if (_playerEntities.Keys.FirstOrDefault(p => p.EntityId == packet.entityId) != null)
			{
				return null;
			}

			return packet;
		}

		private void SendNameTag(Player player)
		{
			player.SetNameTag(TextUtils.Center($"{GetNameTag(player)}\n{ChatColors.Red}HP: {ChatColors.White}{player.HealthManager.Hearts}"));
		}

		private string GetNameTag(Player player)
		{
			return GetNameTag(player.Username);
		}

		private string GetNameTag(string username)
		{
			string rank = username.StartsWith("gurun") || username.StartsWith("Oliver") ? $"{ChatColors.Red}[ADMIN]" : $"{ChatColors.LightPurple}[VIP]";
			return $"{rank} {username}";
		}

		[PacketHandler, Send]
		public void SendUpdateAttributes(McpeUpdateAttributes packet, Player player)
		{
			SendNameTag(player);
		}

		[PacketHandler, Receive]
		public Package MessageHandler(McpeText message, Player player)
		{
			string text = message.message;
			if (text.StartsWith("/") || text.StartsWith("."))
			{
				return message;
			}

			text = TextUtils.RemoveFormatting(text);
			player.Level.BroadcastMessage($"{GetNameTag(player)}:{ChatColors.White} {text}", MessageType.Raw);

			return null;
		}


		private void DoDevelopmentPopups(object state)
		{
			foreach (var level in Context.LevelManager.Levels)
			{
				var players = level.GetSpawnedPlayers();
				foreach (var player in players)
				{
					player.AddPopup(new Popup()
					{
						MessageType = MessageType.Tip,
						Message = "This is a MiNET development server",
						Duration = 20*4
					});

					player.AddPopup(new Popup()
					{
						MessageType = MessageType.Popup,
						Message = "Restarts without notice frequently",
						Duration = 20*5,
						DisplayDelay = 20*1
					});
				}
			}
		}

		[Command]
		public void Fuck(Player player)
		{
			//player.SendSetHealth();
			player.Level.BroadcastMessage(string.Format("{0} current health is {1} with {2} hearts!", player.Username, player.HealthManager.Health, player.HealthManager.Hearts), type: MessageType.Raw);
			player.HealthManager.Health -= 5;
			player.SendUpdateAttributes();
			player.Level.BroadcastMessage(string.Format("{0} health after reset is {1} with {2} hearts!", player.Username, player.HealthManager.Health, player.HealthManager.Hearts), type: MessageType.Raw);
		}

		[Command]
		public void Reset(Player player)
		{
			Level level = player.Level;
			lock (level.Entities)
			{
				foreach (var entity in level.Entities.Values.ToArray())
				{
					entity.DespawnEntity();
				}
				foreach (var entity in level.BlockEntities.ToArray())
				{
					level.RemoveBlockEntity(entity.Coordinates);
				}
			}

			lock (level.Players)
			{
				AnvilWorldProvider worldProvider = level._worldProvider as AnvilWorldProvider;
				if (worldProvider == null) return;

				level.BroadcastMessage(string.Format("{0} resets the world!", player.Username), type: MessageType.Raw);

				lock (worldProvider._chunkCache)
				{
					worldProvider._chunkCache.Clear();
				}

				var players = level.Players;
				foreach (var p in players)
				{
					p.Value.CleanCache();
				}
			}
		}


		[Command]
		public void Awk(Player player)
		{
			string awk = "[" + ChatColors.DarkRed + "AWK" + ChatFormatting.Reset + "]";
			if (player.NameTag.StartsWith(awk))
			{
				player.SetNameTag(player.Username);
				;
			}
			else
			{
				player.SetNameTag(awk + player.Username);
			}
		}

		[Command]
		public void Idk(Player player)
		{
			player.Level.BroadcastMessage(string.Format(ChatColors.Gold + "{0} says 'I don't know' in a nasty way!", player.Username), type: MessageType.Raw);
		}

		[Command]
		public void Lol(Player player)
		{
			player.Level.BroadcastMessage(string.Format(ChatColors.Yellow + "{0} is really 'laughing out loud!', and it really hurst our ears :-(", player.Username), type: MessageType.Raw);
		}

		[Command]
		public void Hi(Player player)
		{
			player.SendMessage(string.Format(ChatColors.Yellow + "Hi {0}!", player.Username), type: MessageType.Raw);
		}


		[Command]
		public void Wtf(Player player)
		{
			player.Level.BroadcastMessage(string.Format(ChatColors.Red + "{0} just said the forbidden 'What the ****'. Shame on {0}!", player.Username), type: MessageType.Raw);
		}

		[Command]
		public void Kick(Player player, string otherUser)
		{
			player.Level.BroadcastMessage(string.Format(ChatColors.Gold + "{0} tried to kick {1} but kicked self instead!!", player.Username, otherUser), type: MessageType.Raw);
			player.Disconnect("You kicked yourself :-)");
		}

		[Command]
		public void Ban(Player player, string otherUser)
		{
			player.Level.BroadcastMessage(string.Format(ChatColors.Gold + "{0} tried to ban {1} but banned self instead!!", player.Username, otherUser), type: MessageType.Raw);
			player.Disconnect("Oopps, banned the wrong player. See ya soon!!");
		}

		[Command]
		public void Hide(Player player)
		{
			HidePlayer(player, true);
			player.Level.BroadcastMessage(string.Format("Player {0} hides.", player.Username), type: MessageType.Raw);
		}

		[Command]
		public void Unhide(Player player)
		{
			HidePlayer(player, false);
			player.Level.BroadcastMessage(string.Format("Player {0} unhides.", player.Username), type: MessageType.Raw);
		}

		private void HidePlayer(Player player, bool hide)
		{
			Player existingPlayer = _playerEntities.Keys.FirstOrDefault(p => p.Username.Equals(player.Username));
			if (existingPlayer != null)
			{
				Entity entity;
				if (_playerEntities.TryGetValue(existingPlayer, out entity))
				{
					_playerEntities.Remove(existingPlayer);
					entity.DespawnEntity();
				}
			}

			Level level = player.Level;
			if (hide)
			{
				player.DespawnFromPlayers(level.GetSpawnedPlayers());
			}
			else
			{
				player.SpawnToPlayers(level.GetSpawnedPlayers());
			}
		}

		[Command]
		public void Spawn(Player player, int mobTypeId)
		{
			Mob mob = new Mob(mobTypeId, player.Level);
			mob.SpawnEntity();
		}

		[Command(Command = "sp")]
		public void SpawnPlayer(Player player, string name)
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			byte[] bytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "IMG_0220.png"));
			//byte[] bytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "Char8.png"));

			PlayerMob fake = new PlayerMob("§6§lBot: " + name + "", player.Level)
			{
				Skin = new Skin {Slim = false, Texture = bytes},
				KnownPosition = player.KnownPosition,
				ItemInHand = new ItemDiamondSword(),
				Helmet = 302,
				Chest = 303,
				Leggings = 304,
				Boots = 305,
			};

			fake.SpawnEntity();
		}

		private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

		[Command]
		public void Hide(Player player, string type)
		{
			EntityType mobType;
			try
			{
				mobType = (EntityType) Enum.Parse(typeof (EntityType), type, true);
			}
			catch (ArgumentException e)
			{
				return;
			}

			Level level = player.Level;

			HidePlayer(player, true);

			Mob entity = new Mob(mobType, level)
			{
				KnownPosition = player.KnownPosition,
				HealthManager = player.HealthManager,
				NameTag = player.NameTag,
			};
			entity.SpawnEntity();

			var remove = McpeRemoveEntity.CreateObject();
			remove.entityId = entity.EntityId;
			player.SendPackage(remove);

			_playerEntities[player] = entity;

			level.BroadcastMessage($"Player {player.Username} spawned as {mobType}.", type: MessageType.Raw);
		}

		[PacketHandler, Receive]
		public Package HandleIncoming(McpeMovePlayer packet, Player player)
		{
			if (_playerEntities.ContainsKey(player))
			{
				var entity = _playerEntities[player];
				entity.KnownPosition = player.KnownPosition;
				var message = McpeMoveEntity.CreateObject();
				message.entities = new EntityLocations();
				message.entities.Add(entity.EntityId, entity.KnownPosition);
				player.Level.RelayBroadcast(message);
			}

			return packet; // Process
		}

		[Command(Command = "w")]
		public void Warp(Player player, string warp)
		{
			float x;
			float y;
			float z;

			switch (warp)
			{
				case "sg1":
					x = 137;
					y = 20;
					z = 431;
					break;
				case "sg2":
					x = 682;
					y = 20;
					z = 324;
					break;
				case "sg3":
					x = 685;
					y = 20;
					z = -119;
					break;
				default:
					return;
			}

			var playerLocation = new PlayerLocation
			{
				X = x,
				Y = y,
				Z = z,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91
			};

			ThreadPool.QueueUserWorkItem(delegate(object state) { player.SpawnLevel(player.Level, playerLocation); }, null);

			//player.Level.BroadcastMessage(string.Format("{0} teleported to coordinates {1},{2},{3}.", player.Username, x, y, z), type: MessageType.Raw);
		}


		[Command]
		[Authorize(Users = "gurun")]
		[Authorize(Users = "gurunx")]
		public void VideoX(Player player, int numberOfFrames, bool color)
		{
			Task.Run(delegate
			{
				try
				{
					Dictionary<Tuple<int, int>, MapEntity> entities = new Dictionary<Tuple<int, int>, MapEntity>();

					int width = 1;
					int height = 1;
					int frameCount = numberOfFrames;
					//int frameOffset = 0;
					int frameOffset = 120;

					var frameTicker = new FrameTicker(frameCount);


					// 768x384
					for (int frame = frameOffset; frame < frameCount + frameOffset; frame++)
					{
						Log.Info($"Generating frame {frame}");

						string file = Path.Combine(@"D:\Development\Other\Smash Heroes 3x6 (128)\Smash Heroes 3x6 (128)", $"Smash Heroes Trailer{frame:D4}.bmp");
						//string file = Path.Combine(@"D:\Development\Other\2 by 1 PE test app ad for Gurun-2\exported frames 2", $"pe app ad{frame:D2}.bmp");
						if (!File.Exists(file)) continue;

						Bitmap image = new Bitmap((Bitmap) Image.FromFile(file), width*128, height*128);

						for (int x = 0; x < width; x++)
						{
							for (int y = 0; y < height; y++)
							{
								var key = new Tuple<int, int>(x, y);
								if (!entities.ContainsKey(key))
								{
									entities.Add(key, new MapEntity(player.Level) {ImageProvider = new VideoImageProvider(frameTicker)});
								}

								var croppedImage = CropImage(image, new Rectangle(new Point(x*128, y*128), new Size(128, 128)));
								byte[] bitmapToBytes = BitmapToBytes(croppedImage, color);

								if (bitmapToBytes.Length != 128*128*4) return;

								((VideoImageProvider) entities[key].ImageProvider).Frames.Add(CreateCachedPacket(entities[key].EntityId, bitmapToBytes));
							}
						}
					}

					int i = 0;

					player.Inventory.Slots[i++] = new ItemBlock(new Planks(), 0) {Count = 64};
					player.Inventory.Slots[i++] = new ItemItemFrame {Count = 64};

					foreach (MapEntity entity in entities.Values)
					{
						entity.SpawnEntity();
						player.Inventory.Slots[i++] = new ItemMap(entity.EntityId);
					}

					player.SendPlayerInventory();
					player.SendMessage("Done generating video.", MessageType.Raw);
				}
				catch (Exception e)
				{
					Log.Error("Aborted video generation", e);
				}
			});

			player.SendMessage("Generating video...", MessageType.Raw);
		}

		[Command]
		[Authorize(Users = "gurun")]
		[Authorize(Users = "gurunx")]
		public void Video2X(Player player, int numberOfFrames, bool color)
		{
			Task.Run(delegate
			{
				try
				{
					Dictionary<Tuple<int, int>, List<MapEntity>> entities = new Dictionary<Tuple<int, int>, List<MapEntity>>();

					int width = 3;
					int height = 2;
					int frameCount = numberOfFrames;
					//int frameOffset = 0;
					int frameOffset = 120;

					var frameTicker = new FrameTicker(frameCount);

					// 768x384
					for (int frame = frameOffset; frame < frameCount + frameOffset; frame++)
					{
						Log.Info($"Generating frame {frame}");

						string file = Path.Combine(@"D:\Development\Other\Smash Heroes 3x6 (128)\Smash Heroes 3x6 (128)", $"Smash Heroes Trailer{frame:D4}.bmp");
						//string file = Path.Combine(@"D:\Development\Other\2 by 1 PE test app ad for Gurun-2\exported frames 2", $"pe app ad{frame:D2}.bmp");
						if (!File.Exists(file)) continue;

						Bitmap image = new Bitmap((Bitmap) Image.FromFile(file), width*128, height*128);

						for (int x = 0; x < width; x++)
						{
							for (int y = 0; y < height; y++)
							{
								var key = new Tuple<int, int>(x, y);
								if (!entities.ContainsKey(key))
								{
									entities.Add(key, new List<MapEntity>());
								}

								List<MapEntity> frames = entities[key];

								var croppedImage = CropImage(image, new Rectangle(new Point(x*128, y*128), new Size(128, 128)));
								byte[] bitmapToBytes = BitmapToBytes(croppedImage, color);

								if (bitmapToBytes.Length != 128*128*4) return;

								MapEntity entity = new MapEntity(player.Level);
								entity.ImageProvider = new MapImageProvider {Batch = CreateCachedPacket(entity.EntityId, bitmapToBytes)};
								entity.SpawnEntity();
								frames.Add(entity);
							}
						}
					}

					int i = 0;

					player.Inventory.Slots[i++] = new ItemBlock(new Planks(), 0) {Count = 64};

					foreach (var entites in entities.Values)
					{
						player.Inventory.Slots[i++] = new CustomItemItemFrame(entites, frameTicker) {Count = 64};
					}

					player.SendPlayerInventory();
					player.SendMessage("Done generating video.", MessageType.Raw);

					BlockCoordinates center = player.KnownPosition.GetCoordinates3D();
					var level = player.Level;

					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							var key = new Tuple<int, int>(x, y);
							List<MapEntity> frames = entities[key];

							BlockCoordinates bc = new BlockCoordinates(center.X - x, center.Y + height - y - 1, center.Z + 2);
							var wood = new Planks {Coordinates = bc};
							level.SetBlock(wood);

							BlockCoordinates frambc = new BlockCoordinates(center.X - x, center.Y + height - y - 1, center.Z + 1);
							ItemFrameBlockEntity itemFrameBlockEntity = new ItemFrameBlockEntity
							{
								Coordinates = frambc
							};

							var itemFrame = new CustomItemFrame(frames, itemFrameBlockEntity, level, frameTicker) {Coordinates = frambc, Metadata = 3};
							level.SetBlock(itemFrame);
							level.SetBlockEntity(itemFrameBlockEntity);
						}
					}
				}
				catch (Exception e)
				{
					Log.Error("Aborted video generation", e);
				}
			});

			player.SendMessage("Generating video...", MessageType.Raw);
		}


		private McpeBatch CreateCachedPacket(long mapId, byte[] bitmapToBytes)
		{
			MapInfo mapInfo = new MapInfo
			{
				MapId = mapId,
				UpdateType = 6,
				Direction = 0,
				X = 0,
				Z = 0,
				Col = 128,
				Row = 128,
				XOffset = 0,
				ZOffset = 0,
				Data = bitmapToBytes,
			};

			McpeClientboundMapItemData packet = McpeClientboundMapItemData.CreateObject();
			packet.mapinfo = mapInfo;
			var batch = CreateMcpeBatch(packet.Encode());

			return batch;
		}

		internal static McpeBatch CreateMcpeBatch(byte[] bytes)
		{
			MemoryStream memStream = MiNetServer.MemoryStreamManager.GetStream();
			memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
			memStream.Write(bytes, 0, bytes.Length);

			McpeBatch batch = McpeBatch.CreateObject();
			byte[] buffer = Player.CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
			batch.MarkPermanent();
			batch.payloadSize = buffer.Length;
			batch.payload = buffer;
			batch.Encode(true);
			return batch;
		}


		private static Bitmap CropImage(Bitmap img, Rectangle cropArea)
		{
			return img.Clone(cropArea, img.PixelFormat);
		}

		private static byte[] ReadFrame(string filename)
		{
			Bitmap bitmap;
			try
			{
				bitmap = new Bitmap(filename);
			}
			catch (Exception e)
			{
				Log.Error("Failed reading file " + filename);
				bitmap = new Bitmap(128, 128);
			}

			byte[] bytes = BitmapToBytes(bitmap);

			return bytes;
		}

		public Bitmap GrayScale(Bitmap bmp)
		{
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					var c = bmp.GetPixel(x, y);
					var rgb = (int) ((c.R + c.G + c.B)/3);
					bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
				}
			}
			return bmp;
		}

		private static byte[] BitmapToBytes(Bitmap bitmap, bool useColor = false)
		{
			byte[] bytes;
			bytes = new byte[bitmap.Height*bitmap.Width*4];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color color = bitmap.GetPixel(x, y);
					if (!useColor)
					{
						byte rgb = (byte) ((color.R + color.G + color.B)/3);
						bytes[i++] = rgb;
						bytes[i++] = rgb;
						bytes[i++] = rgb;
						bytes[i++] = 0xff;
					}
					else
					{
						bytes[i++] = color.R;
						bytes[i++] = color.G;
						bytes[i++] = color.B;
						bytes[i++] = 0xff;
					}
				}
			}
			return bytes;
		}
	}
}