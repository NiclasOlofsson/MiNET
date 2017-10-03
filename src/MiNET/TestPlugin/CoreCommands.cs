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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.Passive;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.UI;
using MiNET.Utils;
using MiNET.Worlds;
using Button = MiNET.UI.Button;
using Input = MiNET.UI.Input;

namespace TestPlugin
{
	[Plugin(PluginName = "CoreCommands", Description = "The core commands for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class CoreCommands : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CoreCommands));

		private Dictionary<string, Level> _worlds = new Dictionary<string, Level>();

		protected override void OnEnable()
		{
			//Context.PluginManager.LoadCommands(new HelpCommand(Context.Server.PluginManager));
			Context.PluginManager.LoadCommands(new VanillaCommands(Context.Server.PluginManager));
		}

		//[PacketHandler, Receive, UsedImplicitly]
		//public Package ReceivePacketHandler(Package packet, Player player)
		//{
		//    Log.Warn($"Receive packet: {packet.GetType().Name}");
		//    return packet;
		//}

		//[PacketHandler, Send, UsedImplicitly]
		//public Package SendPacketHandler(Package packet, Player player)
		//{
		//    Log.Warn($"Sent packet: {packet.GetType().Name}");
		//    return packet;
		//}

		[Command(Name = "tc", Description = "Test command")]
		public void TestCommand(Player player, int param)
		{
		}

		[Command(Description = "Save world")]
		[Authorize(Permission = CommandPermission.Admin)]
		public void Save(Player player)
		{
			AnvilWorldProvider provider = player.Level.WorldProvider as AnvilWorldProvider;
			if (provider != null)
			{
				provider.SaveChunks();
			}
		}

		[Command]
		public void SpawnAgent(Player player, string text)
		{
			Agent agent = new Agent(player.Level);
			agent.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			agent.Owner = player;
			agent.NameTag = text;
			agent.SpawnEntity();
		}

		[Command]
		public void SpawnNpc(Player player, string text)
		{
			Npc npc = new Npc(player.Level);
			npc.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			npc.NameTag = text;
			npc.SpawnEntity();
		}

		[Command]
		public VanillaCommands.SimpleResponse Info(Player player)
		{
			var level = player.Level;
			int entityCount = level.Entities.Count;

			string body = $"Entity #{entityCount}";
			return new VanillaCommands.SimpleResponse() {Body = body};
		}

		[Command]
		public void Relight(Player player)
		{
			BlockCoordinates pos = player.KnownPosition.GetCoordinates3D();
			pos.Y -= 1;

			var block = player.Level.GetBlock(pos);
			Glowstone gold = new Glowstone();
			gold.Coordinates = block.Coordinates;
			player.Level.SetBlock(gold);
			Thread.Sleep(100);
			player.Level.SetBlock(block);
		}

		[Command]
		public void Form(Player player)
		{
			CustomForm customForm = new CustomForm();
			customForm.Title = "A title";
			customForm.Content = new List<CustomElement>()
			{
				new Label {Text = "A label"},
				new Input {Text = "", Placeholder = "Placeholder", Value = ""},
				new Toggle {Text = "A toggler", Value = true},
				new Slider {Text = "A slider", Min = 0, Max = 10, Step = 0.1f, Value = 3},
				new StepSlider {Text = "A step slider", Steps = new List<string>() {"Step 1", "Step 2", "Step 3"}, Value = 1},
				new Dropdown {Text = "A dropdown", Options = new List<string>() {"Option 1", "Option 2", "Option 3"}, Value = 1},
			};

			player.SendForm(customForm);
		}

		[Command]
		public void FormModal(Player player)
		{
			var modalForm = new ModalForm();
			modalForm.Title = "A title";
			modalForm.Content = "A bit of content";
			modalForm.Button1 = "Button 1";
			modalForm.Button2 = "Button 2";

			player.SendForm(modalForm);
		}

		[Command]
		public void FormSimple(Player player)
		{
			var simpleForm = new SimpleForm();
			simpleForm.Title = "A title";
			simpleForm.Content = "A bit of content";
			simpleForm.Buttons = new List<Button>()
			{
				new Button {Text = "Button 1", Image = new Image {Type = "url", Url = "https://i.imgur.com/SedU2Ad.png"}},
				new Button {Text = "Button 2", Image = new Image {Type = "url", Url = "https://i.imgur.com/oBMg5H3.png"}},
				new Button {Text = "Button 3", Image = new Image {Type = "url", Url = "https://i.imgur.com/hMAfqQd.png"}},
				new Button {Text = "Close"},
			};

			player.SendForm(simpleForm);
		}

		[Command]
		public void Minet(Player player, string commands, string done, string gurun, string made, string it)
		{
		}

		[Command(Aliases = new[] {"resend"})]
		public void ResendChunks(Player player)
		{
			Task.Run(() =>
			{
				player.CleanCache();
				player.ForcedSendChunks(() => { player.SendMessage($"Resent chunks."); });
			});
		}

		[Command(Aliases = new[] {"cslc"})]
		public void CalculateSkyLightForChunk(Player player)
		{
			Task.Run(() =>
			{
				Stopwatch sw = new Stopwatch();
				var level = player.Level;
				ChunkColumn chunk = level.GetChunk((BlockCoordinates) player.KnownPosition);
				sw.Start();
				new SkyLightCalculations().RecalcSkyLight(chunk, level);
				sw.Stop();
				player.CleanCache();
				player.ForcedSendChunks(() => { player.SendMessage($"Calculated skylights ({sw.ElapsedMilliseconds}ms) and resent chunks."); });
			});
		}

		[Command(Aliases = new[] {"csl"})]
		public void CalculateSkyLight(Player player)
		{
			Task.Run(() =>
			{
				SkyLightCalculations.Calculate(player.Level);
				player.CleanCache();
				player.ForcedSendChunks(() => { player.SendMessage("Calculated skylights and resent chunks."); });
			});
		}

		[Command(Aliases = new[] {"cbl"})]
		public void CalculateBlockLight(Player player)
		{
			Task.Run(() =>
			{
				LevelManager.RecalculateBlockLight(player.Level, (AnvilWorldProvider) player.Level.WorldProvider);
				player.CleanCache();
				player.ForcedSendChunks(() => { player.SendMessage("Calculated blocklights and resent chunks."); });
			});
		}


		[Command(Name = "le")]
		public void LevelEvent(Player player, short value)
		{
			LevelEvent(player, value, 0);
		}

		[Command(Name = "le")]
		public void LevelEvent(Player player, short value, int data)
		{
			McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			levelEvent.eventId = value;
			levelEvent.data = data;
			levelEvent.position = player.KnownPosition.ToVector3();
			player.Level.RelayBroadcast(levelEvent);

			player.Level.BroadcastMessage($"Sent level event {value}", type: MessageType.Raw);
		}

		[Command(Name = "td")]
		public void ToggleDownfall(Player player, int value)
		{
			{
				McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
				levelEvent.eventId = 3001;
				levelEvent.data = value;
				player.SendPackage(levelEvent);
			}
			player.SendMessage("Downfall " + value, type: MessageType.Raw);
		}

		[Command]
		public void ToggleDownfall(Player player)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				for (int i = 0; i < short.MaxValue; i = i + 2000)
				{
					var data = i;
					{
						McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
						levelEvent.eventId = 3001;
						levelEvent.data = data;
						player.SendPackage(levelEvent);
					}
					//{
					//	McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
					//	levelEvent.eventId = 3002;
					//	levelEvent.data = i;
					//	player.SendPackage(levelEvent);
					//}
					player.SendMessage("Downfall " + data, type: MessageType.Raw);
					Thread.Sleep(5000);
				}
				for (int i = short.MaxValue; i >= 0; i = i - 2000)
				{
					{
						McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
						levelEvent.eventId = 3001;
						levelEvent.data = i;
						player.SendPackage(levelEvent);
					}
					//{
					//	McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
					//	levelEvent.eventId = 3002;
					//	levelEvent.data = i;
					//	player.SendPackage(levelEvent);
					//}

					player.SendMessage("Downfall " + i, type: MessageType.Raw);
					Thread.Sleep(5000);
				}
			});

			//{
			//	McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			//	levelEvent.eventId = 3001;
			//	levelEvent.data = 100000;
			//	player.SendPackage(levelEvent);
			//}
			//{
			//	McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			//	levelEvent.eventId = 3002;
			//	levelEvent.data = 36625;
			//	player.SendPackage(levelEvent);
			//}
			player.SendMessage("Toggled downfall", type: MessageType.Raw);
		}


		[Command]
		public void Version(Player player)
		{
			string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
			player.SendMessage($"MiNET v{productVersion}", type: MessageType.Raw);
		}

		[Command]
		public void Params(Player player, params string[] args)
		{
			player.SendMessage($"Executed command params, got {args.Length} arguments", type: MessageType.Raw);
			foreach (string s in args)
			{
				player.SendMessage($"{s}", type: MessageType.Raw);
			}
		}


		[Command]
		public void Plugins(Player player)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Plugins: ");
			foreach (var plugin in Context.PluginManager.Plugins)
			{
				sb.AppendLine(plugin.GetType().Name);
			}

			player.SendMessage(sb.ToString(), type: MessageType.Raw);
		}

		[Command]
		public void Portal(Player player)
		{
			int width = 4;
			int height = 5;

			int x = (int) player.KnownPosition.X - width/2;
			int y = (int) player.KnownPosition.Y - 1;
			int z = (int) player.KnownPosition.Z + 1;

			PortalInfo portal = new PortalInfo();
			portal.Coordinates = new BlockCoordinates(x, y, z);
			portal.Size = new BoundingBox(portal.Coordinates, portal.Coordinates + new BlockCoordinates(4, 5, 3));
			Player.BuildPortal(player.Level, portal);
		}

		[Command]
		public void ReadTest(Player player)
		{
			int width = 128;
			int height = player.Level.Dimension == Dimension.Overworld ? 256 : 128;


			Stopwatch sw = new Stopwatch();
			sw.Start();
			Level level = player.Level;
			int blockId = new Portal().Id;
			BlockCoordinates start = (BlockCoordinates) player.KnownPosition;
			for (int x = start.X - width; x < start.X + width; x++)
			{
				for (int z = start.Z - width; z < start.Z + width; z++)
				{
					for (int y = height - 1; y >= 0; y--)
					{
						var b = level.IsBlock(new BlockCoordinates(x, y, z), blockId);
						if (b) Log.Warn("Found portal block");
					}
				}
			}
			sw.Stop();

			player.SendMessage($"Read blocks in {sw.ElapsedMilliseconds}ms");
		}


		[Command]
		public void Orb(Player player1)
		{
			foreach (Player player in player1.Level.Players.Values)
			{
				// 128 = 32 + 32 + 32
				var msg = McpeSpawnExperienceOrb.CreateObject();
				msg.position = player1.KnownPosition.ToVector3() + new Vector3(1, 2, 1);
				msg.count = 10;
				player.Level.RelayBroadcast(msg);
			}
		}

		[Command(Name = "dim", Aliases = new[] {"dimension"}, Description = "Change dimension. Creates world if not exist.")]
		public void ChangeDimenion(Player player, DimensionTypesEnum dimType)
		{
			Dimension dimension;
			switch (dimType.Value)
			{
				case "overworld":
					dimension = Dimension.Overworld;
					break;
				case "nether":
					dimension = Dimension.Nether;
					break;
				case "the_end":
					dimension = Dimension.TheEnd;
					break;
				default:
					return;
			}

			Level oldLevel = player.Level;

			if (player.Level.Dimension == dimension)
			{
				player.Teleport(player.SpawnPosition);
				return;
			}

			if (!Context.LevelManager.Levels.Contains(player.Level))
			{
				Context.LevelManager.Levels.Add(player.Level);
			}

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				player.ChangeDimension(null, null, dimension, delegate
				{
					Level nextLevel = dimension == Dimension.Overworld ? oldLevel.OverworldLevel : dimension == Dimension.Nether ? oldLevel.NetherLevel : oldLevel.TheEndLevel;
					return nextLevel;
				});

				oldLevel.BroadcastMessage(string.Format("{0} teleported to world {1}.", player.Username, player.Level.LevelId), type: MessageType.Raw);
			}, Context.LevelManager);
		}

		[Command(Name = "tpw", Aliases = new[] {"teleport"}, Description = "Teleports player to default world.")]
		public void TeleportWorld(Player player)
		{
			TeleportWorld(player, "Default");
		}

		private object _levelSync = new object();

		[Command(Name = "tpw", Aliases = new[] {"teleport"}, Description = "Teleports player to given world. Creates world if not exist.")]
		public void TeleportWorld(Player player, string world)
		{
			Level oldLevel = player.Level;

			if (player.Level.LevelId.Equals(world))
			{
				player.Teleport(player.SpawnPosition);
				return;
			}

			if (!Context.LevelManager.Levels.Contains(player.Level))
			{
				Context.LevelManager.Levels.Add(player.Level);
			}

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				LevelManager levelManager = state as LevelManager;
				if (levelManager == null) return;

				Level[] levels = levelManager.Levels.ToArray();

				if (levels != null)
				{
					player.SpawnLevel(null, null, true, delegate
					{
						lock (levelManager.Levels)
						{
							Level nextLevel = levels.FirstOrDefault(l => l.LevelId != null && l.LevelId.Equals(world));

							if (nextLevel == null)
							{
								nextLevel = new Level(levelManager, world, new AnvilWorldProvider() {MissingChunkProvider = new FlatlandWorldProvider()}, Context.LevelManager.EntityManager, player.GameMode, Difficulty.Normal);
								nextLevel.Initialize();
								Context.LevelManager.Levels.Add(nextLevel);
							}

							oldLevel.BroadcastMessage(string.Format("{0} teleported to world {1}.", player.Username, player.Level.LevelId), type: MessageType.Raw);

							return nextLevel;
						}
					});
				}
			}, Context.LevelManager);
		}

		[Command]
		public void Clear(Player player)
		{
			for (byte slot = 0; slot < 35; slot++) player.Inventory.SetInventorySlot(slot, new ItemAir()); //Empty all slots.
		}

		[Command]
		public void Clear(Player player, Player target)
		{
			Clear(target);
		}

		[Command(Name = "vd")]
		public void ViewDistance(Player player)
		{
			player.Level.BroadcastMessage(string.Format("Current view distance set to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Name = "vd")]
		public void ViewDistance(Player player, int viewDistance)
		{
			player.Level.ViewDistance = viewDistance;
			player.Level.BroadcastMessage(string.Format("View distance changed to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Name = "twitter")]
		public void Twitter(Player player)
		{
			player.Level.BroadcastMessage("�6Twitter @NiclasOlofsson", type: MessageType.Raw);
			player.Level.BroadcastMessage("�5twitch.tv/gurunx", type: MessageType.Raw);
		}

		[Command(Name = "pi")]
		public void PlayerInfo(Player player)
		{
			player.SendMessage(string.Format("Username={0}", player.Username), type: MessageType.Raw);
			player.SendMessage(string.Format("Entity ID={0}", player.EntityId), type: MessageType.Raw);
			player.SendMessage(string.Format("Client ID={0}", player.ClientId), type: MessageType.Raw);
			player.SendMessage(string.Format("Client ID={0}", player.ClientUuid), type: MessageType.Raw);
		}

		[Command(Name = "pos")]
		public void Position(Player player)
		{
			BlockCoordinates position = new BlockCoordinates(player.KnownPosition);

			int chunkX = position.X >> 4;
			int chunkZ = position.Z >> 4;

			int xi = position.X & 0x0f;
			int zi = position.Z & 0x0f;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Position X={player.KnownPosition.X:F1} Y={player.KnownPosition.Y:F1} Z={player.KnownPosition.Z:F1}");
			sb.AppendLine($"Direction Yaw={player.KnownPosition.Yaw:F1} HeadYap={player.KnownPosition.HeadYaw:F1} Pitch={player.KnownPosition.Pitch:F1}");
			sb.AppendLine($"Region X={chunkX >> 5} Z={chunkZ >> 5}");
			sb.AppendLine($"Chunk X={chunkX} Z={chunkZ}");
			sb.AppendLine($"Local coordinates X={xi} Z={zi}");
			sb.AppendLine($"Local coordinates X={xi} Z={zi}");
			sb.AppendLine($"Height={player.Level.GetHeight(position)}");
			string text = sb.ToString();

			player.SendMessage(text, type: MessageType.Raw);
			Log.Info(text);
		}

		[Command]
		public void Permission(Player player, int permission)
		{
			player.CommandPermission = (CommandPermission) permission;
			player.SendAdventureSettings();
		}


		[Command]
		public void Spawn(Player player, int entityId)
		{
			Level level = player.Level;

			var entity = new Entity(entityId, level)
			{
				KnownPosition = player.KnownPosition,
			};

			entity.SpawnEntity();
		}

		[Command]
		public void Doit(Player player, bool isAngry, byte data)
		{
			Level level = player.Level;

			var entity = new Wolf(level)
			{
				KnownPosition = player.KnownPosition,
				IsAngry = isAngry,
				CollarColor = data,
			};

			entity.SpawnEntity();
		}

		[Command]
		public void Strike(Player player)
		{
			//player.Level.StrikeLightning(player.KnownPosition.ToVector3());
			player.StrikeLightning();
		}

		[Command(Name = "si")]
		public void SendInventory(Player player)
		{
			player.SendPlayerInventory();
		}

		[Command(Name = "sp")]
		public void SetSpawn(Player player)
		{
			player.SpawnPosition = (PlayerLocation) player.KnownPosition.Clone();
			player.SendSetSpawnPosition();
			player.Level.BroadcastMessage($"{player.Username} set new spawn position.", type: MessageType.Raw);
		}

		[Command]
		public void Ignite(Player player)
		{
			player.HealthManager.Ignite();
		}

		[Command]
		public void FarmingKit(Player player)
		{
			var inventory = player.Inventory;

			var command = new ItemCommand(41, 0, delegate(ItemCommand itemCommand, Level level, Player arg3, BlockCoordinates arg4) { Log.Info("Clicked on command"); });

			byte c = 0;
			inventory.Slots[c++] = new ItemDiamondHoe();
			inventory.Slots[c++] = new ItemBucket(8) {Count = 1};
			inventory.Slots[c++] = new ItemWheatSeeds() {Count = 64};
			inventory.Slots[c++] = new ItemBeetrootSeeds() {Count = 64};
			inventory.Slots[c++] = new ItemCarrot() {Count = 64};
			inventory.Slots[c++] = new ItemPotato() {Count = 64};


			player.SendPlayerInventory();
			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastMessage(string.Format("Player {0} changed kit.", player.Username), type: MessageType.Raw);
		}

		[Command]
		public void Kit(Player player, int kitId)
		{
			var inventory = player.Inventory;

			switch (kitId)
			{
				case 0:
					// Kit leather tier
					inventory.Boots = new ItemLeatherBoots();
					inventory.Leggings = new ItemLeatherLeggings();
					inventory.Chest = new ItemLeatherChestplate();
					inventory.Helmet = new ItemLeatherHelmet();
					break;
				case 1:
					// Kit gold tier
					inventory.Boots = new ItemGoldBoots();
					inventory.Leggings = new ItemGoldLeggings();
					inventory.Chest = new ItemGoldChestplate();
					inventory.Helmet = new ItemGoldHelmet();
					break;
				case 2:
					// Kit chain tier
					inventory.Boots = new ItemChainmailBoots();
					inventory.Leggings = new ItemChainmailLeggings();
					inventory.Chest = new ItemChainmailChestplate();
					inventory.Helmet = new ItemChainmailHelmet();
					break;
				case 3:
					// Kit iron tier
					inventory.Boots = new ItemIronBoots();
					inventory.Leggings = new ItemIronLeggings();
					inventory.Chest = new ItemIronChestplate();
					inventory.Helmet = new ItemIronHelmet();
					break;
				case 4:
					// Kit diamond tier
					inventory.Boots = new ItemDiamondBoots();
					inventory.Leggings = new ItemDiamondLeggings();
					inventory.Chest = new ItemDiamondChestplate();
					inventory.Helmet = new ItemDiamondHelmet();
					break;
			}

			// 0 = protection
			// 1 = Fire protection
			// 2 = Feather falling
			// 3 = Blast protection
			// 4 = Projectile protection
			// 5 = Thorns

			EnchantArmor(player.Inventory, (short) EnchantingType.FireProtection, 7);


			var command = new ItemCommand(41, 0, delegate(ItemCommand itemCommand, Level level, Player arg3, BlockCoordinates arg4) { Log.Info("Clicked on command"); });

			// Hotbar
			byte c = 0;
			//inventory.Slots[c++] = new ItemStack(command, 1); // Custom command block

			//2016 - 02 - 26 02:59:08,740[6] INFO MiNET.Client.MiNetClient - Item Type = Item, Id = 358, Metadata = 0, Count = 1, 
			// ExtraData = TAG_Compound("tag"): 1 entries {
			//	TAG_String("map_uuid"): "-4294967268"
			//}

			inventory.Slots[c++] = new ItemBow() {ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 4)}}}}; // Bow
			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound
				{
					new NbtList("ench")
					{
						new NbtCompound {new NbtShort("id", (short) EnchantingType.Knockback), new NbtShort("lvl", 1)}
					}
				}
			};
			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound
				{
					new NbtList("ench")
					{
						new NbtCompound {new NbtShort("id", (short) EnchantingType.Knockback), new NbtShort("lvl", 2)}
					}
				}
			};
			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound
				{
					new NbtList("ench")
					{
						new NbtCompound {new NbtShort("id", (short) EnchantingType.Knockback), new NbtShort("lvl", 3)}
					}
				}
			};
			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound
				{
					new NbtList("ench")
					{
						new NbtCompound {new NbtShort("id", (short) EnchantingType.Knockback), new NbtShort("lvl", 4)}
					}
				}
			};
			inventory.Slots[c++] = new ItemEnchantingTable();
			inventory.Slots[c++] = ItemFactory.GetItem(351, 4, 64);
			inventory.Slots[c++] = new ItemBlock(new Planks(), 0) {Count = 64};
			inventory.Slots[c++] = new ItemCompass(); // Wooden Sword
			inventory.Slots[c++] = new ItemWoodenSword(); // Wooden Sword
			inventory.Slots[c++] = new ItemStoneSword(); // Stone Sword
			inventory.Slots[c++] = new ItemGoldSword(); // Golden Sword
			inventory.Slots[c++] = new ItemIronSword(); // Iron Sword
			inventory.Slots[c++] = new ItemDiamondSword(); // Diamond Sword
			inventory.Slots[c++] = new ItemArrow {Count = 64}; // Arrows
			inventory.Slots[c++] = new ItemEgg {Count = 64}; // Eggs
			inventory.Slots[c++] = new ItemSnowball {Count = 64}; // Snowballs
			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound
				{
					new NbtList("ench")
					{
						new NbtCompound {new NbtShort("id", (short) EnchantingType.FireAspect), new NbtShort("lvl", 1)},
						new NbtCompound {new NbtShort("id", (short) EnchantingType.Knockback), new NbtShort("lvl", 1)}
					}
				}
			};

			inventory.Slots[c++] = new ItemIronSword
			{
				ExtraData = new NbtCompound {{new NbtCompound("display") {new NbtString("Name", "test")}}}
			};

			//inventory.Slots[c++] = new ItemEmptyMap { Count = 64 }; // Wooden Sword
			inventory.Slots[c++] = new ItemStoneAxe();
			inventory.Slots[c++] = new ItemStoneAxe();
			inventory.Slots[c++] = new ItemWoodenPickaxe();
			inventory.Slots[c++] = new ItemBread {Count = 5};
			//inventory.Slots[c++] = new ItemBlock(new Block(35), 0) {Count = 64};
			//inventory.Slots[c++] = new ItemBucket(8);

			//inventory.Slots[c++] = ItemFactory.GetItem(39, 0) { Count = 1};
			//inventory.Slots[c++] = ItemFactory.GetItem(40, 0), 4);
			//inventory.Slots[c++] = ItemFactory.GetItem(281, 0), 4);

			//for (byte i = 0; i < inventory.ItemHotbar.Length; i++)
			//{
			//	inventory.ItemHotbar[i] = i;
			//}

			player.SendPlayerInventory();
			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastMessage(string.Format("Player {0} changed kit.", player.Username), type: MessageType.Raw);
		}

		private void EnchantArmor(PlayerInventory inventory, short enchId, short level)
		{
			inventory.Helmet.ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", enchId), new NbtShort("lvl", level)}}};
			inventory.Chest.ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", enchId), new NbtShort("lvl", level)}}};
			inventory.Leggings.ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", enchId), new NbtShort("lvl", level)}}};
			inventory.Boots.ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", enchId), new NbtShort("lvl", level)}}};
		}

		[Command]
		public void Potions(Player player)
		{
			var inventory = player.Inventory;

			byte c = 0;
			//inventory.Slots[c++] = new ItemStack(command, 1); // Custom command block
			for (short i = 5; i < 36; i++)
			{
				inventory.Slots[c++] = new ItemPotion(i);
			}

			player.SendPlayerInventory();
			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastMessage($"{player.Username} got potions.", type: MessageType.Raw);
		}

		[Command]
		public void Furnace(Player player)
		{
			var inventory = player.Inventory;

			byte c = 0;
			inventory.Slots[c++] = new ItemFurnace() {Count = 64}; // Custom command block
			inventory.Slots[c++] = new ItemCoal() {Count = 64}; // Custom command block
			inventory.Slots[c++] = new ItemBlock(new IronOre(), 0) {Count = 64}; // Custom command block

			player.SendPlayerInventory();
			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastMessage($"{player.Username} got potions.", type: MessageType.Raw);
		}


		private void SendEquipmentForPlayer(Player player)
		{
			var msg = McpeMobEquipment.CreateObject();
			msg.runtimeEntityId = player.EntityId;
			msg.item = player.Inventory.GetItemInHand();
			msg.slot = 0;
			player.Level.RelayBroadcast(msg);
		}

		private void SendArmorForPlayer(Player player)
		{
			var armorEquipment = McpeMobArmorEquipment.CreateObject();
			armorEquipment.runtimeEntityId = player.EntityId;
			armorEquipment.helmet = player.Inventory.Helmet;
			armorEquipment.chestplate = player.Inventory.Chest;
			armorEquipment.leggings = player.Inventory.Leggings;
			armorEquipment.boots = player.Inventory.Boots;
			player.Level.RelayBroadcast(armorEquipment);
		}

		[Command]
		public void StringTest(Player player, string str1, string str2)
		{
		}

		[Command]
		public void OptionalTest(Player player, int x, int y, int z, int rot = 0)
		{
		}

		[Command]
		public void EnumTest(Player player, ItemTypeEnum itemType, EntityTypeEnum entityType, BlockTypeEnum blockType, CommandNameEnum commandName)
		{
		}

		[Command]
		public void EnumTest2(Player player, EnchantEnum enchant, EffectEnum effect)
		{
		}

		[Command(Description = "Fly command")]
		public void Fly(Player player)
		{
			if (player.AllowFly)
			{
				player.SetAllowFly(false);
				player.Level.BroadcastMessage($"Player {player.Username} disabled flying.", type: MessageType.Raw);
			}
			else
			{
				player.SetAllowFly(true);
				player.Level.BroadcastMessage($"Player {player.Username} enabled flying.", type: MessageType.Raw);
			}
		}

		[Command(Name = "xporb")]
		public void ExperienceOrb(Player player)
		{
			Mob xpOrb = new Mob(69, player.Level);
			xpOrb.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
			xpOrb.SpawnEntity();
		}

		[Command(Name = "e")]
		public void Effect(Player player, string effect)
		{
			if ("clear".Equals(effect, StringComparison.InvariantCultureIgnoreCase))
			{
				player.Level.BroadcastMessage($"Removed all effects for {player.Username}.", MessageType.Raw);
				player.RemoveAllEffects();
			}
		}

		[Command(Name = "e")]
		public void Effect(Player player, string effect, int level)
		{
			Effect(player, effect, level, MiNET.Effects.Effect.MaxDuration);
		}

		[Command(Name = "e")]
		public void Effect(Player player, string effect, int level, int duration)
		{
			if ("clear".Equals(effect, StringComparison.InvariantCultureIgnoreCase))
			{
				player.Level.BroadcastMessage($"Removed all effects for {player.Username}.", MessageType.Raw);
				player.RemoveAllEffects();
				return;
			}

			EffectType effectType;
			if (Enum.TryParse(effect, true, out effectType))
			{
				Effect eff = null;
				switch (effectType)
				{
					case EffectType.Speed:
						eff = new Speed();
						break;
					case EffectType.Slowness:
						eff = new Slowness();
						break;
					case EffectType.Haste:
						eff = new Haste();
						break;
					case EffectType.MiningFatigue:
						eff = new MiningFatigue();
						break;
					case EffectType.Strength:
						eff = new Strength();
						break;
					case EffectType.InstantHealth:
						eff = new InstantHealth();
						break;
					case EffectType.InstantDamage:
						eff = new InstantDamage();
						break;
					case EffectType.JumpBoost:
						eff = new JumpBoost();
						break;
					case EffectType.Nausea:
						eff = new Nausea();
						break;
					case EffectType.Regeneration:
						eff = new Regeneration();
						break;
					case EffectType.Resistance:
						eff = new Resistance();
						break;
					case EffectType.FireResistance:
						eff = new FireResistance();
						break;
					case EffectType.WaterBreathing:
						eff = new WaterBreathing();
						break;
					case EffectType.Invisibility:
						eff = new Invisibility();
						break;
					case EffectType.Blindness:
						eff = new Blindness();
						break;
					case EffectType.NightVision:
						eff = new NightVision();
						break;
					case EffectType.Hunger:
						eff = new Hunger();
						break;
					case EffectType.Weakness:
						eff = new Weakness();
						break;
					case EffectType.Poison:
						eff = new Poison();
						break;
					case EffectType.Wither:
						eff = new Wither();
						break;
					case EffectType.HealthBoost:
						eff = new HealthBoost();
						break;
					case EffectType.Absorption:
						eff = new Absorption();
						break;
					case EffectType.Saturation:
						eff = new Saturation();
						break;
				}

				if (eff != null)
				{
					eff.Level = level;
					eff.Duration = duration;
					eff.Particles = false;

					player.SetEffect(eff);
					player.Level.BroadcastMessage($"{player.Username} added effect {effectType} with strenght {level}", MessageType.Raw);
				}
			}
		}

		[Command(Name = "nd")]
		public void NoDamage(Player player)
		{
			player.HealthManager = player.HealthManager is NoDamageHealthManager ? new HealthManager(player) : new NoDamageHealthManager(player);
			player.SendUpdateAttributes();
			player.SendMessage($"{player.Username} set NoDamage={player.HealthManager is NoDamageHealthManager}", type: MessageType.Raw);
		}

		[Command(Name = "r")]
		//[Authorize(Permission = UserPermission.Op)]
		public void DisplayRestartNotice(Player currentPlayer)
		{
			var players = currentPlayer.Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				player.AddPopup(new Popup()
				{
					Priority = 100,
					MessageType = MessageType.Tip,
					Message = "SERVER WILL RESTART!",
					Duration = 20*10,
				});

				player.AddPopup(new Popup()
				{
					Priority = 100,
					MessageType = MessageType.Popup,
					Message = "Transfering all players!",
					Duration = 20*10,
				});
			}

			foreach (var player in players)
			{
				McpeTransfer transfer = McpeTransfer.CreateObject();
				transfer.serverAddress = "yodamine.com";
				transfer.port = 19132;
				player.SendPackage(transfer);
			}
		}

		private byte _invId = 0;

		[Command(Name = "oi")]
		public void OpenInventory(Player player)
		{
			BlockCoordinates coor = new BlockCoordinates(player.KnownPosition);
			Chest chest = new Chest
			{
				Coordinates = coor,
				Metadata = 0
			};
			player.Level.SetBlock(chest, true);

			// Then we create and set the sign block entity that has all the intersting data

			ChestBlockEntity chestBlockEntity = new ChestBlockEntity
			{
				Coordinates = coor
			};

			player.Level.SetBlockEntity(chestBlockEntity, false);

			player.OpenInventory(coor);
		}

		[Command]
		public void Test1(Player player)
		{
			List<Pig> pigs = new List<Pig>();
			for (int i = 0; i < 10; i++)
			{
				Pig pig = new Pig(player.Level);
				pig.KnownPosition = (PlayerLocation) player.KnownPosition.Clone();
				pig.SpawnEntity();
				pigs.Add(pig);
			}
			player.SendMessage("Spawned pigs");

			Thread.Sleep(4000);

			PlayerLocation loc = (PlayerLocation) player.KnownPosition.Clone();
			loc.Y = loc.Y + 10;
			loc.X = loc.X + 10;
			loc.Z = loc.Z + 10;

			player.SendMessage("Moved pigs");

			Thread.Sleep(4000);

			foreach (var pig in pigs)
			{
				pig.KnownPosition = (PlayerLocation) loc.Clone();
				pig.LastUpdatedTime = DateTime.UtcNow;
			}

			player.SendMessage("Moved ALL pigs");
		}

		[Command]
		public void Test2(Player player)
		{
			PlayerLocation pos = (PlayerLocation) player.KnownPosition.Clone();
			Task.Run(() =>
			{
				for (int i = 0; i < 100; i++)
				{
					pos.HeadYaw += 10;
					pos.Yaw += 10;
					player.SetPosition(pos);
					Thread.Sleep(100);
				}
			});
		}

		[Command]
		public void Title(Player player, string text, TitleType type)
		{
			player.SendTitle(text, type);
		}


		[Command]
		public void Count(Player player)
		{
			List<string> users = new List<string>();
			var levels = Context.Server.LevelManager.Levels;
			foreach (var level in levels)
			{
				foreach (var spawnedPlayer in level.GetSpawnedPlayers())
				{
					users.Add(spawnedPlayer.Username);
				}
			}

			player.SendMessage($"There are {users.Count} of players online.");
		}

		[Command]
		public VanillaCommands.SimpleResponse Worldborder(Player player, int radius = 200, bool centerOnPlayer = false)
		{
			Level level = player.Level;

			BlockCoordinates center = (BlockCoordinates) level.SpawnPoint;
			if (centerOnPlayer) center = (BlockCoordinates) player.KnownPosition;
			center.Y = 0;

			var players = level.GetSpawnedPlayers();

			for (int y = 0; y < 256; y++)
			{
				for (int x = -radius; x <= radius; x++)
				{
					for (int z = -radius; z <= radius; z++)
					{
						if (x != -radius && x != radius && z != -radius && z != radius) continue;

						var block = new Glass() {Coordinates = center + new BlockCoordinates(x, y, z)};
						level.SetBlock(block, false, false, false);

						List<Player> sendList = new List<Player>();
						foreach (var p in players)
						{
							if (p.KnownPosition.DistanceTo(center + new BlockCoordinates(x, (int) p.KnownPosition.Y, z)) > p.ChunkRadius*16) continue;

							sendList.Add(p);
						}

						var message = McpeUpdateBlock.CreateObject();
						message.blockId = block.Id;
						message.coordinates = block.Coordinates;
						message.blockMetaAndPriority = (byte) (0xb << 4 | (block.Metadata & 0xf));
						level.RelayBroadcast(sendList.ToArray(), message);
					}
				}
			}
			return new VanillaCommands.SimpleResponse() {Body = $"Added world border with radius of {radius} around {center}"};
		}

		[Command]
		public VanillaCommands.SimpleResponse Test()
		{
			return new VanillaCommands.SimpleResponse() {Body = "Test"};
		}

		[Command]
		public void GeneratePath(Player player)
		{
			Level level = player.Level;
			Vector3 pos = player.KnownPosition;

			int n = 20;

			RandomCurve ycurve = new RandomCurve(n, 0, 80, 0.1);
			RandomCurve zcurve = new RandomCurve(n, 0, 100, 0.1);

			bool first = true;
			for (int x = 0; x < n; x++)
			{
				var y = ycurve.GetY(x);
				var z = zcurve.GetY(x);

				GeneratePortal(level, pos + new Vector3(x*42, (float) y, (float) z), first, x == n - 1);
				first = false;
			}
		}

		private void GeneratePortal(Level level, BlockCoordinates coord, bool isStart = false, bool isLast = false)
		{
			Block block = isStart ? (Block) new DiamondBlock() : isLast ? (Block) new EmeraldBlock() : new GoldBlock();

			int[,] coords = new[,]
			{
				{0, 0}, {0, 1}, {0, 2}, {0, 3}, {0, 4}, {0, 0},
				{1, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {1, 5},
				{2, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {2, 5},
				{3, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {3, 5},
				{4, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {4, 5},
				{5, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {5, 5},
				{0, 0}, {6, 1}, {6, 2}, {6, 3}, {6, 4}, {6, 5},
			};

			for (int i = 0; i < coords.Length/2; i++)
			{
				Log.Warn($"Lenght {coords.Length}");
				block.Coordinates = coord + new BlockCoordinates(0, coords[i, 0], coords[i, 1]);
				level.SetBlock(block);
			}
		}
	}


	internal struct SineWave
	{
		internal readonly double Amplitude;
		internal readonly double OrdinaryFrequency;
		internal readonly double AngularFrequency;
		internal readonly double Phase;
		internal readonly double ShiftY;

		internal SineWave(double amplitude, double ordinaryFrequency, double phase, double shiftY)
		{
			Amplitude = amplitude;
			OrdinaryFrequency = ordinaryFrequency;
			AngularFrequency = 2*Math.PI*ordinaryFrequency;
			Phase = phase;
			ShiftY = shiftY;
		}
	}

	public class RandomCurve
	{
		private SineWave[] m_sineWaves;

		public RandomCurve(int components, double minY, double maxY, double flatness)
		{
			m_sineWaves = new SineWave[components];

			double totalPeakToPeakAmplitude = maxY - minY;
			double averagePeakToPeakAmplitude = totalPeakToPeakAmplitude/components;

			int prime = 1;
			Random r = new Random();
			for (int i = 0; i < components; i++)
			{
				// from 0.5 to 1.5 of averagePeakToPeakAmplitude 
				double peakToPeakAmplitude = averagePeakToPeakAmplitude*(r.NextDouble() + 0.5d);

				// peak amplitude is a hald of peak-to-peak amplitude
				double amplitude = peakToPeakAmplitude/2d;

				// period should be a multiple of the prime number to avoid regularities
				prime = Utils.GetNextPrime(prime);
				double period = flatness*prime;

				// ordinary frequency is reciprocal of period
				double ordinaryFrequency = 1d/period;

				// random phase
				double phase = 2*Math.PI*(r.NextDouble() + 0.5d);

				// shiftY is the same as amplitude
				double shiftY = amplitude;

				m_sineWaves[i] =
					new SineWave(amplitude, ordinaryFrequency, phase, shiftY);
			}
		}

		public double GetY(double x)
		{
			double y = 0;
			for (int i = 0; i < m_sineWaves.Length; i++)
				y += m_sineWaves[i].Amplitude*Math.Sin(m_sineWaves[i].AngularFrequency*x + m_sineWaves[i].Phase) + m_sineWaves[i].ShiftY;

			return y;
		}
	}

	internal static class Utils
	{
		internal static int GetNextPrime(int i)
		{
			int nextPrime = i + 1;
			for (; !IsPrime(nextPrime); nextPrime++) ;
			return nextPrime;
		}

		private static bool IsPrime(int number)
		{
			if (number == 1) return false;
			if (number == 2) return true;

			for (int i = 2; i < number; ++i)
				if (number%i == 0) return false;

			return true;
		}
	}
}