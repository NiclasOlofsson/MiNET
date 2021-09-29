#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Sounds;
using MiNET.Utils;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace TestPlugin.NiceLobby
{
	[Plugin(PluginName = "NiceLobby", Description = "", PluginVersion = "1.0", Author = "MiNET Team")]
	public class NiceLobbyPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(NiceLobbyPlugin));

		private Timer _popupTimer;
		private Timer _tickTimer;

		private long _tick = 0;

		protected override void OnEnable()
		{
			var server = Context.Server;

			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				//BossBar bossBar = new BossBar(level)
				//{
				//	Animate = false,
				//	MaxProgress = 10,
				//	Progress = 10,
				//	NameTag = $"{ChatColors.Gold}You are playing on a {ChatColors.Gold}MiNET{ChatColors.Gold} server"
				//};
				//bossBar.SpawnEntity();

				//level.AllowBuild = false;
				//level.AllowBreak = false;

				//level.BlockBreak += LevelOnBlockBreak;
				//level.BlockPlace += LevelOnBlockPlace;
			};

			server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				Player player = args.Player;
				player.PlayerJoin += OnPlayerJoin;
				player.LocalPlayerIsInitialized += OnLocalPlayerIsInitialized;
				player.PlayerLeave += OnPlayerLeave;
				player.Ticking += OnTicking;
			};

			//_popupTimer = new Timer(DoDevelopmentPopups, null, 10000, 20000);
			//_tickTimer = new Timer(LevelTick, null, 0, 50);
			//_tickTimer = new Timer(SkinTick, null, 0, 50);
		}

		[Command]
		public void TestTranslatedMessages(Player player)
		{
			for (var i = 0; i <= 8; i++)
			{
				player.SendMessage(ChatColors.Green + (MessageType) i + ChatFormatting.Reset + ": %key.smoothCamera", (MessageType) i, player, true);
			}
		}

		private void OnTicking(object sender, PlayerEventArgs e)
		{
			var player = e.Player;
			var level = player.Level;

			if (level.TickTime % 10 == 0 && player.Inventory.GetItemInHand() is CustomTestItem item)
			{
				player.SendMessage("0x" + item.SomeVariable.ToString("X"), MessageType.Popup);
			}

			//if (e.Level.TickTime % 2 == 0)
			//{
			//	BlockCoordinates pos = (BlockCoordinates)player.KnownPosition;
			//	player.AddPopup(new Popup()
			//	{
			//		Id = 11,
			//		MessageType = MessageType.Popup,
			//		Message = $"SkyLight Subtracted={level.SkylightSubtracted}, Under={level.GetSkyLight(pos + BlockCoordinates.Down)}, Foot={level.GetSkyLight(pos)}, Head={level.GetSkyLight(pos + BlockCoordinates.Up)}, Height={level.GetHeight(pos)}",
			//		Duration = 20 * 5,
			//	});
			//}

			// Compass
			//if (e.Level.TickTime % 2 == 0)
			//{
			//	player.AddPopup(new Popup()
			//	{
			//		Id = 10,
			//		MessageType = MessageType.Tip,
			//		Message = GetCompass(player.KnownPosition.HeadYaw),
			//		Duration = 20 * 5,
			//	});
			//}


			// Glide extension
			//if (player.IsGliding)
			//{
			//	if (player.CurrentSpeed > 30)
			//	{
			//		var particle = new CriticalParticle(level);
			//		particle.Position = player.KnownPosition.ToVector3();
			//		particle.Spawn();
			//	}

			//	if (level.TickTime%10 == 0)
			//	{
			//		player.AddPopup(new Popup()
			//		{
			//			Id = 10,
			//			MessageType = MessageType.Tip,
			//			Message = $"Speed: {player.CurrentSpeed:F2}m/s",
			//			Duration = 20*5,
			//		});
			//	}
			//}
		}

		public static float Wrap(float angle)
		{
			return (float) (angle + Math.Ceiling(-angle / 360) * 360);
		}

		public static string GetCompass(float direction)
		{
			direction = Wrap(direction);
			direction = direction * 2 / 10;

			direction += 72;

			int width = 25;

			var compass = new string('|', 72).ToCharArray();
			compass[0] = 'S';

			compass[9] = 'S';
			compass[9 + 1] = 'W';

			compass[(18)] = 'W';

			compass[(18 + 9)] = 'N';
			compass[(18 + 9 + 1)] = 'W';

			compass[36] = 'N';

			compass[36 + 9] = 'N';
			compass[36 + 9 + 1] = 'E';

			compass[54] = 'E';

			compass[54 + 9] = 'S';
			compass[54 + 9 + 1] = 'E';

			compass = compass.Concat(compass).Concat(compass).ToArray();

			return new String(compass.Skip((int) (direction - Math.Floor((double) width / 2))).Take(width).ToArray())
					.Replace("|", "| ")
					.Replace("| N|", $"| {ChatFormatting.Bold}{ChatColors.Red}N{ChatFormatting.Reset} |")
					.Replace("| NE|", $"| {ChatFormatting.Bold}{ChatColors.Yellow}NE{ChatFormatting.Reset} |").Trim('N', 'W', 'S', 'E').Trim('N', 'W', 'S', 'E')
					.Replace("| E|", $"| {ChatFormatting.Bold}{ChatColors.Green}E{ChatFormatting.Reset} |")
					.Replace("| SE|", $"| {ChatFormatting.Bold}{ChatColors.Green}SE{ChatFormatting.Reset} |")
					.Replace("| S|", $"| {ChatFormatting.Bold}{ChatColors.Aqua}S{ChatFormatting.Reset} |")
					.Replace("| SW|", $"| {ChatFormatting.Bold}{ChatColors.Blue}SW{ChatFormatting.Reset} |")
					.Replace("| W|", $"| {ChatFormatting.Bold}{ChatColors.DarkPurple}W{ChatFormatting.Reset} |")
					.Replace("| NW|", $"| {ChatFormatting.Bold}{ChatColors.LightPurple}NW{ChatFormatting.Reset} |")
				;
		}


		private object _skinSynk = new object();

		private int _image = 0;
		private int _imageCape = 0;

		private void SkinTick(object state)
		{
			if (!Monitor.TryEnter(_skinSynk)) return;

			try
			{
				foreach (var player in _players.Values)
				{
					{
						if (!player.Username.Equals("gurunx")) continue;

						Log.Debug("Skin tick");

						if (_image >= 9) _image = 0;

						_image++;
						_imageCape++;

						Skin skin = player.Skin;


						//skin.SkinGeometryName = "";
						//skin.SkinGeometry = Encoding.UTF8.GetBytes(File.ReadAllText(@"D:\Temp\humanoid.json"));

						//{
						//	string file = Path.Combine(@"D:\Development\Other\Smash Heroes 3x6 (128)\Smash Heroes 3x6 (128)", $"Smash Heroes Trailer{_imageCape:D4}.bmp");
						//	//string file = @"D:\Temp\Smiley\big_smile0" + _image + ".png";
						//	if (!File.Exists(file))
						//	{
						//		_imageCape = 0;
						//		continue;
						//	}

						//	//Bitmap bitmap = new Bitmap((Bitmap)Image.FromFile(file), 12, 18);
						//	Bitmap bitmap = new Bitmap((Bitmap) Image.FromFile(file), 64, 64);
						//	int offsetx = 16, offsety = 16;
						//	bitmap = CropImage(bitmap, new Rectangle(offsetx, offsety, 12, 18));
						//	byte[] bytes = new byte[32 * 64 * 4];

						//	int i = 0;
						//	for (int y = 0; y < 32; y++)
						//	{
						//		for (int x = 0; x < 64; x++)
						//		{
						//			if (y >= bitmap.Height || x >= bitmap.Width)
						//			{
						//				Color color = Color.Yellow;
						//				bytes[i++] = color.R;
						//				bytes[i++] = color.G;
						//				bytes[i++] = color.B;
						//				bytes[i++] = color.A;
						//				continue;
						//			}
						//			else
						//			{
						//				Color color = bitmap.GetPixel(x, y);
						//				bytes[i++] = color.R;
						//				bytes[i++] = color.G;
						//				bytes[i++] = color.B;
						//				bytes[i++] = color.A;
						//			}
						//		}
						//	}
						//	skin.Cape = new Cape()
						//	{
						//		ImageHeight = 32,
						//		ImageWidth = 64,
						//		Data = bytes,
						//	};
						//}


						Level level = player.Level;
						if (level.TickTime % 10 != 0) continue;
						//player.SetNameTag(player.Username + " " + level.TickTime + " testing");
						//player.SetDisplayName(player.Username + " " + level.TickTime + " testing");

						var texture = skin.Data;
						byte[] smiley = GetTextureFromFile(@"C:\Temp\Smiley\big_smile0" + _image + ".png");
						if (smiley.Length != 8 * 8 * 4) return;
						int s = 0;
						int br = 8;
						int bc = 8;
						for (int r = 0; r < 8; r++)
						{
							for (int c = 0; c < 8; c++)
							{
								int i = ((c + bc) * 4) + ((r + br) * 64 * 4);
								int j = ((c) * 4) + ((r) * 8 * 4);

								texture[(i) + 0] = smiley[j + 0];
								texture[(i) + 1] = smiley[j + 1];
								texture[(i) + 2] = smiley[j + 2];
								texture[(i) + 3] = smiley[j + 3];
							}
						}

						{
							var updateSkin = McpePlayerSkin.CreateObject();
							updateSkin.uuid = player.ClientUuid;
							updateSkin.oldSkinName = player.Skin.SkinId;
							updateSkin.skin = skin;
							updateSkin.skinName = skin.SkinId;
							level.RelayBroadcast(updateSkin);
						}

						{
							//player.SpawnPosition = player.KnownPosition;

							//level.DespawnFromAll(player);
							//level.SpawnToAll(player);

							//var players = level.GetSpawnedPlayers();

							//McpePlayerList playerList = McpePlayerList.CreateObject();
							//playerList.records = new PlayerAddRecords {player};
							//level.RelayBroadcast(player, players, CreateMcpeBatch(playerList.Encode()));
							//playerList.records = null;
							//playerList.PutPool();

							//player.IsInvisible = true;
							//player.HideNameTag = true;
							//player.BroadcastSetEntityData();

							//player.SpawnToPlayers(players);

							//Thread.Sleep(100);
							//player.HideNameTag = false;
							//player.IsInvisible = false;
							//player.BroadcastSetEntityData();
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.Error("SkinTick", e);
			}
			finally
			{
				Monitor.Exit(_skinSynk);
			}
		}

		public static byte[] GetTextureFromFile(string filename)
		{
			Bitmap bitmap = new Bitmap(filename);
			byte[] bytes = new byte[bitmap.Height * bitmap.Width * 4];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color color = bitmap.GetPixel(x, y);
					bytes[i++] = color.R;
					bytes[i++] = color.G;
					bytes[i++] = color.B;
					bytes[i++] = color.A;
				}
			}

			return bytes;
		}

		private ConcurrentDictionary<string, Player> _players = new ConcurrentDictionary<string, Player>();

		private void OnPlayerJoin(object o, PlayerEventArgs eventArgs)
		{
			Level level = eventArgs.Level;
			if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

			Player player = eventArgs.Player;
			if (player == null) throw new ArgumentNullException(nameof(eventArgs.Player));

			if (player.CertificateData.ExtraData.Xuid != null && player.Username.Equals("gurunx"))
			{
				player.ActionPermissions = ActionPermissions.Operator;
				player.CommandPermission = 4;
				player.PermissionLevel = PermissionLevel.Operator;
				player.SendAdventureSettings();
			}

			//player.HealthManager.MaxHealth = 800;
			//player.HealthManager.ResetHealth();
			//player.SendUpdateAttributes();


			int idx = 0;
			//player.Inventory.Slots[idx++] = new ItemMonsterEgg(EntityType.Sheep) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemMonsterEgg(EntityType.Wolf) {Count = 64};

			//player.Inventory.Slots[idx++] = new ItemEmptyMap()
			//{
			//	Count = 64,
			//	UniqueId = Environment.TickCount
			//};
			player.Inventory.Slots[idx++] = new ItemAir()
			{
				Count = 0,
				UniqueId = 0
			};
			player.Inventory.Slots[idx++] = new ItemDiamondAxe() {Count = 1};
			player.Inventory.Slots[idx++] = new ItemDiamondShovel() {Count = 1};
			player.Inventory.Slots[idx++] = new ItemDiamondPickaxe() {Count = 1};
			player.Inventory.Slots[idx++] = new ItemBlock(new CraftingTable()) {Count = 64};
			player.Inventory.Slots[idx++] = new ItemBlock(new Chest()) {Count = 64};
			player.Inventory.Slots[idx++] = new ItemBlock(new Anvil()) {Count = 64};
			player.Inventory.Slots[idx++] = new ItemBlock(new EnchantingTable()) {Count = 64};
			player.Inventory.Slots[idx++] = new ItemBlock(new Loom()) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemBlock(new Sapling()) { Count = 64 };
			//player.Inventory.Slots[idx++] = new ItemBlock(new Sapling(), 2) { Count = 64 };
			//player.Inventory.Slots[idx++] = new ItemBlock(new Vine(), 0) { Count = 64 };
			//player.Inventory.Slots[idx++] = new ItemBlock(new SnowLayer()) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemBlock(new Dirt()) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemBlock(new WoodenButton(), 0) { Count = 64 };
			//player.Inventory.Slots[idx++] = new CustomTestItem(0xC0FFEE) { Count = 1 };
			//player.Inventory.Slots[idx++] = new CustomTestItem(0xDEADBEEF) {Count = 10 };
			//player.Inventory.Slots[idx++] = new CustomTestItem(0xDEADBEEF) {Count = 10 };
			//player.Inventory.Slots[idx++] = new CustomTestItem(0xBEEF) {Count = 10 };
			idx = 8;
			player.Inventory.Slots[idx++] = new ItemStick() {Count = 64};

			var fireworks = new ItemFireworkRocket() {Count = 64};

			fireworks.ExtraData = ItemFireworkRocket.ToNbt(new ItemFireworkRocket.FireworksData()
			{
				Explosions = new List<ItemFireworkRocket.FireworksExplosion>()
				{
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 0},
						FireworkFade = new[] {(byte) 1},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 0,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 1},
						FireworkFade = new[] {(byte) 2},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 1,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 2},
						FireworkFade = new[] {(byte) 3},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 2,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 3},
						FireworkFade = new[] {(byte) 4},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 3,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 4},
						FireworkFade = new[] {(byte) 5},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 4,
					}
				},
				Flight = 1
			});

			player.Inventory.Slots[idx++] = fireworks;
			//player.Inventory.Slots[idx++] = new ItemBread() {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemSnowball() {Count = 16};
			//player.Inventory.Slots[idx++] = new ItemBow() {Count = 1};
			//player.Inventory.Slots[idx++] = new ItemArrow() {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemBlock(new Torch(), 0) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemBlock(new Stone(), 0) {Count = 64};
			//player.Inventory.Slots[idx++] = new ItemWheat() {Count = 1};
			//player.Inventory.Slots[idx++] = new ItemCarrot() {Count = 1};
			//player.Inventory.Slots[idx++] = new ItemWheatSeeds() {Count = 1};
			//player.Inventory.Slots[idx++] = new ItemBone() {Count = 64};
			player.Inventory.Slots[idx++] = new ItemDye()
			{
				Metadata = 4,
				Count = 64
			};
			player.Inventory.Slots[idx++] = new ItemIronIngot() {Count = 64};
			player.Inventory.Slots[idx++] = new ItemIronSword()
			{
				Count = 1,
				Metadata = 0,
				UniqueId = Environment.TickCount
			};
			player.Inventory.Slots[idx++] = new ItemIronShovel()
			{
				Count = 1,
				Metadata = 0,
				UniqueId = Environment.TickCount
			};
			player.Inventory.Slots[idx++] = new ItemIronAxe()
			{
				Count = 1,
				Metadata = 0,
				UniqueId = Environment.TickCount
			};
			player.Inventory.Slots[idx++] = new ItemIronPickaxe()
			{
				Count = 1,
				Metadata = 0,
				UniqueId = Environment.TickCount
			};

			player.Inventory.Helmet = new ItemDiamondHelmet() {UniqueId = Environment.TickCount};
			player.Inventory.Chest = new ItemElytra() {UniqueId = Environment.TickCount};
			player.Inventory.Leggings = new ItemDiamondLeggings() {UniqueId = Environment.TickCount};
			player.Inventory.Boots = new ItemDiamondBoots() {UniqueId = Environment.TickCount};
			//while (player.Inventory.SetFirstEmptySlot(new ItemIronAxe(), false)) { }

			player.SendPlayerInventory();

			player.SendArmorForPlayer();
			player.SendEquipmentForPlayer();

			_players.TryAdd(player.Username, player);
		}

		private void OnLocalPlayerIsInitialized(object o, PlayerEventArgs eventArgs)
		{
			Thread.Sleep(1000);
			Player player = eventArgs.Player;
			Level level = eventArgs.Level;

			level.BroadcastMessage($"{ChatColors.Gold}[{ChatColors.Green}+{ChatColors.Gold}]{ChatFormatting.Reset} {player.Username} joined the server");

			var joinSound = new AnvilUseSound(level.SpawnPoint.ToVector3());
			joinSound.Spawn(level);

			//player.SendTitle(null, TitleType.Clear);
			player.SendTitle(null, TitleType.AnimationTimes, 6, 6, 20 * 7); // 7 seconds
			if (Context.Server.IsEdu)
			{
				player.SendTitle($"{ChatColors.White}This is a MiNET Education Edition server", TitleType.SubTitle);
				player.SendTitle($"{ChatColors.Gold}Welcome!", TitleType.Title);
			}
			else
			{
				player.SendTitle($"{ChatColors.White}This is gurun's MiNET test server", TitleType.SubTitle);
				player.SendTitle($"{ChatColors.Gold}Welcome {player.Username}!", TitleType.Title);
			}
		}

		private void OnPlayerLeave(object o, PlayerEventArgs eventArgs)
		{
			Level level = eventArgs.Level;
			if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

			Player player = eventArgs.Player;
			if (player == null) throw new ArgumentNullException(nameof(eventArgs.Player));

			Player trash;
			_players.TryRemove(player.Username, out trash);

			level.BroadcastMessage($"{ChatColors.Gold}[{ChatColors.Red}-{ChatColors.Gold}]{ChatFormatting.Reset} {player.Username} left the server");
			var leaveSound = new AnvilBreakSound(level.SpawnPoint.ToVector3());
			leaveSound.Spawn(level);
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

		private float m = 0.1f;

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
					//timeDay.started = true;
					level.RelayBroadcast(timeDay);

					ThreadPool.QueueUserWorkItem(delegate(object o)
					{
						Thread.Sleep(100);

						McpeSetTime timeReset = McpeSetTime.CreateObject();
						timeReset.time = (int) level.WorldTime;
						level.RelayBroadcast(timeDay);

						//Thread.Sleep(200);

						//{
						//	var mcpeExplode = McpeExplode.CreateObject();
						//	mcpeExplode.position = point1.ToVector3();
						//	mcpeExplode.radius = 100;
						//	mcpeExplode.records = new Records();
						//	level.RelayBroadcast(mcpeExplode);
						//}

						//Thread.Sleep(250);
						//{
						//	var mcpeExplode = McpeExplode.CreateObject();
						//	mcpeExplode.position = point2.ToVector3();
						//	mcpeExplode.radius = 100;
						//	mcpeExplode.records = new Records();
						//	level.RelayBroadcast(mcpeExplode);
						//}
						//Thread.Sleep(250);
						//{
						//	var mcpeExplode = McpeExplode.CreateObject();
						//	mcpeExplode.position = point3.ToVector3();
						//	mcpeExplode.radius = 100;
						//	mcpeExplode.records = new Records();
						//	level.RelayBroadcast(mcpeExplode);
						//}
					});
				}

				if (m < 0.4 || m > 3)
					for (int i = 0; i < 15 + (30 * m); i++)
					{
						GenerateParticles(random, level, point1, m < 0.6 ? 0 : 20, new Vector3(m * (m / 2), m + 10, m * (m / 2)), m);
						GenerateParticles(random, level, point2, m < 0.4 ? 0 : 12, new Vector3(m, m + 6, m), m);
						GenerateParticles(random, level, point3, m < 0.2 ? 0 : 9, new Vector3(m / 2, m / 2 + 6, m / 2), m);
					}
			}
			m += 0.1f;
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
			mobParticles.position = new Vector3(point.X + vx, (point.Y - 2) + yoffset + vy, point.Z + vz);
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

		//[PacketHandler, Receive, UsedImplicitly]
		//public Package LoginHandler(McpeLogin packet, Player player)
		//{
		//	player.DisplayName = TextUtils.Center($"{GetNameTag(packet.username ?? "")}");
		//	return packet;
		//}

		[PacketHandler, Send]
		public Packet RespawnHandler(McpeRespawn packet, Player player)
		{
			SendNameTag(player);
			player.RemoveAllEffects();

			player.SetEffect(new Speed
			{
				Level = 1,
				Duration = Effect.MaxDuration
			}); // 10s in ticks
			//player.SetEffect(new Slowness { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new Haste { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new MiningFatigue { Level = 20, Duration = 20 * 10 });
			//player.SetEffect(new Strength { Level = 20, Duration = 20 * 10 });
			player.SetEffect(new JumpBoost
			{
				Level = 1,
				Duration = Effect.MaxDuration
			});
			//player.SetEffect(new Blindness { Level = 20, Duration = 20 * 10 });
			//player.SetAutoJump(true);

			return packet;
		}

		[PacketHandler, Send]
		public Packet AddPlayerHandler(McpeAddPlayer packet, Player player)
		{
			if (_playerEntities.Keys.FirstOrDefault(p => p.EntityId == packet.entityIdSelf) != null)
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
			string username = player.Username;

			string rank;
			if (username.StartsWith("gurun") || username.StartsWith("Oliver"))
			{
				rank = $"{ChatColors.Red}[ADMIN]";
			}
			else if (player.CertificateData.ExtraData.Xuid != null)
			{
				rank = $"{ChatColors.Green}";
				//rank = $"{ChatColors.Green}[XBOX]";
			}
			else
			{
				rank = $"{ChatColors.White}";
			}

			return $"{rank} {username}";
		}

		[PacketHandler, Send]
		public void SendUpdateAttributes(McpeUpdateAttributes packet, Player player)
		{
			SendNameTag(player);
		}

		[PacketHandler, Receive]
		public Packet MessageHandler(McpeText message, Player player)
		{
			string text = TextUtils.RemoveFormatting(message.message);
			player.Level.BroadcastMessage($"{GetNameTag(player)} says:{ChatColors.White} {text}", MessageType.Chat);
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
						Message = $"{ChatFormatting.Bold}This is a MiNET development server",
						Duration = 20 * 4
					});

					player.AddPopup(new Popup()
					{
						MessageType = MessageType.Popup,
						Message = "Restarts without notice frequently",
						Duration = 20 * 5,
						DisplayDelay = 20 * 1
					});
				}
			}
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
				AnvilWorldProvider worldProvider = level.WorldProvider as AnvilWorldProvider;
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


		private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

		[Command]
		public void Hide(Player player, string type)
		{
			EntityType mobType;
			try
			{
				mobType = (EntityType) Enum.Parse(typeof(EntityType), type, true);
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
			remove.entityIdSelf = entity.EntityId;
			player.SendPacket(remove);

			_playerEntities[player] = entity;

			level.BroadcastMessage($"Player {player.Username} spawned as {mobType}.", type: MessageType.Raw);
		}

		[PacketHandler, Receive]
		public Packet HandleIncoming(McpeMovePlayer packet, Player player)
		{
			if (_playerEntities.ContainsKey(player))
			{
				var entity = _playerEntities[player];
				entity.KnownPosition = player.KnownPosition;
				var message = McpeMoveEntity.CreateObject();
				message.runtimeEntityId = entity.EntityId;
				message.position = entity.KnownPosition;
				player.Level.RelayBroadcast(message);
			}

			return packet; // Process
		}

		[Command(Name = "w")]
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