using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using MiNET;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Net;
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
		[UsedImplicitly] private Timer _popupTimer;
		[UsedImplicitly] private Timer _gameTimer;
		[UsedImplicitly] private Timer _scoreboardTimer;
		[UsedImplicitly] private Timer _tickTimer;

		private long _tick = 0;

		protected override void OnEnable()
		{
			_popupTimer = new Timer(DoDevelopmentPopups, null, 10000, 20000);
			//_gameTimer = new Timer(StartNewRoundCallback, null, 15000, 60000*3);
			//_tickTimer = new Timer(LevelTick, null, 0, 50);
			//foreach (var level in Context.LevelManager.Levels)
			//{
			//	level.BlockBreak += LevelOnBlockBreak;
			//	level.BlockPlace += LevelOnBlockPlace;
			//}
		}

		private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
		{
			e.Cancel = e.Player.GameMode != GameMode.Creative;
		}

		private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
		{
			//e.Cancel = e.Player.GameMode != GameMode.Creative;
		}

		private double m = 0.1d;

		private void LevelTick(object state)
		{
			if (m > 0)
			{
				//if (_tick%random.Next(1, 4) == 0)
				Level level = Context.LevelManager.Levels.First();
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
					return 4; // Expload
					break;
				case 1:
					return 5; // Flame
					break;
				case 2:
					return 6; // Lava
					break;
				case 3:
					return 2; // Critical
					break;
				case 4:
					return 21; // Lava drip
					break;
				case 5:
					return 13; // Entity flame
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

		[PacketHandler, Send, UsedImplicitly]
		public Package RespawnHandler(McpeRespawn packet, Player player)
		{
			player.RemoveAllEffects();

			player.SetEffect(new Speed { Level = 2, Duration = 1000 });
			////player.SetEffect(new Slowness {Level = 2, Duration = 20});
			//player.SetEffect(new JumpBoost { Level = 2, Duration = Effect.MaxDuration });
			//player.SetAutoJump(true);

			if (player.Level.LevelId.Equals("Default"))
			{
				player.Level.CurrentWorldTime = 6000;
				player.Level.IsWorldTimeStarted = false;
			}

			player.SendSetTime();

			return packet;
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
						Message = "This is a development server",
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

		private void StartNewRoundCallback(object state)
		{
			if (_scoreboardTimer == null)
			{
				_scoreboardTimer = new Timer(ScoreboardCallback, null, 5000, 47000);

				Context.LevelManager.Levels[0].BroadcastMessage(
					"§6§l»§r§7 --------------------------- §6§l«\n"
					+ "§e GAME STARTED\n"
					+ "§6§l»§r§7 --------------------------- §6§l«", type: McpeText.TypeRaw);
			}
			else
			{
				var players = Context.LevelManager.Levels[0].GetSpawnedPlayers();
				if (players.Length <= 1) return;

				var winner = players.OrderByDescending(CalculatedKdRatio).FirstOrDefault();

				if (winner != null)
				{
					Context.LevelManager.Levels[0].BroadcastMessage(
						"§6§l»§r§7 --------------------------- §6§l«\n"
						+ "§e Winner!!\n"
						+ "§e       " + winner.Username + "\n", type: McpeText.TypeRaw);
				}
				foreach (var player in players)
				{
					player.Kills = 0;
					player.Deaths = 0;
				}

				Context.LevelManager.Levels[0].BroadcastMessage(
					"§e NEW ROUND STARTED\n"
					+ "§6§l»§r§7 --------------------------- §6§l«", type: McpeText.TypeRaw);
			}
		}

		private static double CalculatedKdRatio(Player player)
		{
			return player.Deaths == 0 ? player.Kills : player.Kills/((double) player.Deaths);
		}

		private void ScoreboardCallback(object state)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("§6§l»§r§7 --------------------------- §6§l«\n");
			var players = Context.LevelManager.Levels[0].GetSpawnedPlayers();
			if (players.Length <= 1) return;

			foreach (var player in players.OrderByDescending(CalculatedKdRatio).Take(5))
			{
				sb.AppendFormat("K/D: {3:0.00} K: {1:00} D: {2:00} {0}\n", player.Username, player.Kills, player.Deaths, CalculatedKdRatio(player));
			}
			sb.Append("§6§l»§r§7 --------------------------- §6§l«\n");
			Context.LevelManager.Levels[0].BroadcastMessage(sb.ToString(), type: McpeText.TypeRaw);
		}

		[Command]
		public void Fuck(Player player)
		{
			//player.SendSetHealth();
			player.Level.BroadcastMessage(string.Format("{0} current health is {1} with {2} hearts!", player.Username, player.HealthManager.Health, player.HealthManager.Hearts), type: MessageType.Raw);
			player.HealthManager.Health -= 5;
			player.SendSetHealth();
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
				player.NameTag = player.Username;
			}
			else
			{
				player.NameTag = awk + player.Username;
			}

			player.BroadcastSetEntityData();
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
			Level level = player.Level;
			if (hide)
			{
				foreach (var pair in level.Players)
				{
					Player targetPlayer = pair.Value;

					level.SendRemoveForPlayer(targetPlayer, player);
				}
			}
			else
			{
				foreach (var targetPlayer in level.GetSpawnedPlayers())
				{
					level.SendAddForPlayer(targetPlayer, player);
				}
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

			PlayerMob fake = new PlayerMob(name, player.Level)
			{
				Skin = new Skin {Slim = false, Texture = bytes},
				KnownPosition = player.KnownPosition,
				ItemInHand = new ItemStack(267),
				Helmet = 302,
				Chest = 303,
				Leggings = 304,
				Boots = 305,
			};

			fake.SpawnEntity();
		}

        private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

        [Command]
        public void Hide(Player player, byte id)
        {
            Level level = player.Level;

            HidePlayer(player, true);

            Mob entity = new Mob(id, level)
            {
                KnownPosition = player.KnownPosition,
                //Data = -(blockId | 0 << 0x10)
            };
            entity.SpawnEntity();

            player.SendPackage(new McpeRemoveEntity()
            {
                entityId = entity.EntityId,
            });

            _playerEntities[player] = entity;

            level.BroadcastTextMessage(string.Format("Player {0} spawned as other entity.", player.Username), type: MessageType.Raw);
        }

        [PacketHandler, Receive]
        public Package HandleIncoming(McpeMovePlayer packet, Player player)
        {
            if (_playerEntities.ContainsKey(player))
            {
                var entity = _playerEntities[player];
                entity.KnownPosition = player.KnownPosition;
                var message = new McpeMoveEntity();
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
	}
}