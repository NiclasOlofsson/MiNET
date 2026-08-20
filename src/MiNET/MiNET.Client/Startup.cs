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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Client
{
	public class Startup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

		// ReSharper disable once InconsistentNaming
		private const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		/// <summary>
		///     What the join burst actually delivered. Columns and blobs both count: the announcement
		///     flow only ever names blobs by hash, so a client holding columns with no sections in
		///     them has answered verdicts for terrain it never resolved. Waits for the stream to go
		///     quiet first, since the burst keeps arriving after spawn.
		/// </summary>
		private static void ReportChunkCache(MiNetClient client)
		{
			// Done means every announced blob is held AND no new column has arrived for a moment.
			// Payloads settle long after the columns that announced them, and columns keep arriving
			// after the last payload of the previous batch, so either test alone reports a client
			// that is still mid-stream. Anything short of that runs the clock out and is reported as
			// it stands.
			int columns = 0, quiet = 0;
			for (int i = 0; i < 60 && client.IsConnected; i++)
			{
				Thread.Sleep(500);
				int next = client.ChunkCache.Columns.Count;
				bool settled = next > 0 && next == columns && client.ChunkCache.Blobs.PayloadCount == client.ChunkCache.Blobs.Count;
				columns = next;
				if (settled && ++quiet >= 3) break;
				if (!settled) quiet = 0;
			}

			int complete = 0, sections = 0, biomes = 0, pulled = 0, pushed = 0, legacy = 0;
			var pushedColumns = new List<ChunkCoordinates>();
			foreach (CachedChunkColumn column in client.ChunkCache.Columns)
			{
				if (column.IsComplete) complete++;
				sections += column.Sections.Count;
				if (column.Biomes != null) biomes++;

				switch (column.Delivery)
				{
					case ChunkDelivery.Pull: pulled++; break;
					case ChunkDelivery.Push:
						pushed++;
						pushedColumns.Add(column.Coordinates);
						break;
					default: legacy++; break;
				}
			}

			// The coordinates, not just the count: whether the server sent exactly the columns that
			// entered the view and nothing else is a question about the set.
			if (pushedColumns.Count > 0)
			{
				pushedColumns.Sort((a, b) => a.X == b.X ? a.Z.CompareTo(b.Z) : a.X.CompareTo(b.X));
				Console.WriteLine($"Pushed columns: {string.Join(" ", pushedColumns.Select(c => $"{c.X},{c.Z}"))}");
			}

			// Anything still queued here was never asked for: verdicts and requests only leave on a
			// flush, and a flush only happens when a packet arrives, so a leftover at rest is a
			// column that will never complete.
			int owed = client.PendingBlobHits.Count + client.PendingBlobMisses.Count + client.PendingSubChunkColumns.Count;

			string report = $"Chunk cache: {columns} columns ({complete} complete, {biomes} with biomes, {pulled} pulled, {pushed} pushed, {legacy} legacy), {sections} sections, {client.ChunkCache.Blobs.PayloadCount} of {client.ChunkCache.Blobs.Count} blobs held, {owed} answers still queued";
			Console.WriteLine(report);
			Log.Info(report);
		}

		/// <summary>
		///     Walks the bot in +X at vanilla walking speed, one PlayerAuthInput per tick, which is the
		///     only movement packet the 1.26 server reads. Sixteen blocks is one chunk border, and a
		///     border crossing is what makes the server stream the next ring.
		/// </summary>
		private static void WalkForward(MiNetClient client, int blocks)
		{
			const float speed = 4.317f; // vanilla walking, blocks per second
			var forward = new Vector2(0, 1);

			PlayerLocation start = client.CurrentLocation;
			var position = new Vector3(start.X, start.Y, start.Z);
			int ticks = (int) (blocks / speed * 20);

			Log.Info($"Walking {blocks} blocks in +X from {position} over {ticks} ticks");

			for (int i = 0; i < ticks; i++)
			{
				Vector3 previous = position;
				position = position with {X = position.X + speed / 20f};

				var input = McpePlayerAuthInput.CreateObject();
				input.playerRotation = new Vector2(0, 90);
				input.playerHeadRotation = 90;
				input.position = position;
				input.moveVector = forward;
				input.inputData = AuthInputFlags.WalkForwards;
				input.inputMode = McpePlayerAuthInput.InputMode.Mouse;
				input.playMode = McpePlayerAuthInput.ClientPlayMode.Normal;
				input.newInteractionModel = McpePlayerAuthInput.NewInteractionModel.Touch;
				input.interactRotation = new Vector2(0, 90);
				input.clientTick = i;
				input.posDelta = position - previous;
				input.analogMoveVector = forward;
				input.rawMoveVector = forward;
				input.cameraOrientation = new Vector3(0, 90, 0);
				client.SendPacket(input);

				client.CurrentLocation = new PlayerLocation(position, 90, 90, 0);
				Thread.Sleep(50);
			}

			Log.Info($"Walk finished at {position}");
		}

		/// <summary>Places one of every block that keeps a block entity, in a line beside the bot, by
		/// asking the server to do it. Nothing is read back here: the server sends the block entity
		/// data by itself, and BedrockTraceHandler prints the tag it arrives in.</summary>
		private static void ProbeContainerBlocks(MiNetClient client)
		{
			string[] blocks =
			{
				"chest", "trapped_chest", "copper_chest", "waxed_oxidized_copper_chest",
				"undyed_shulker_box", "red_shulker_box", "barrel", "smoker", "brewing_stand",
				"hopper", "dispenser", "dropper", "crafter", "furnace", "blast_furnace",
				"ender_chest", "lectern", "chiseled_bookshelf", "decorated_pot", "enchanting_table",
				"beacon"
			};

			Action<Task, string> send = BotHelpers.DoSendCommand(client);

			var origin = (BlockCoordinates) client.CurrentLocation;
			Log.Warn($"Block probe: placing {blocks.Length} blocks from {origin}");

			for (int i = 0; i < blocks.Length; i++)
			{
				int x = origin.X + 2 + i * 2;
				send(null, $"/setblock {x} {origin.Y} {origin.Z + 2} {blocks[i]}");
				Thread.Sleep(500);

				// A container the server considers empty may not be worth a block entity to it.
				send(null, $"/replaceitem block {x} {origin.Y} {origin.Z + 2} slot.container 0 diamond 1");
				Thread.Sleep(500);
			}

			Log.Warn("Block probe: placed, waiting for the server to answer");
			Thread.Sleep(10000);
			Log.Warn("Block probe: done");
		}

		/// <summary>Moves the bot to a place and reports the block the server actually sent for each
		/// coordinate asked about. This is the client's side of a ghost block: when the world file and
		/// the server agree that a chest is there and the player sees an outline with nothing in it,
		/// the question is what went over the wire, and this answers it without a real client.</summary>
		private static void InspectBlocks(MiNetClient client, string spec)
		{
			List<BlockCoordinates> wanted = spec
				.Split(';', StringSplitOptions.RemoveEmptyEntries)
				.Select(part => part.Split(','))
				.Select(p => new BlockCoordinates(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2])))
				.ToList();

			if (wanted.Count == 0) return;

			// The server sends chunks around where it thinks the player is, so stand there first.
			BlockCoordinates first = wanted[0];
			client.CurrentLocation = new PlayerLocation(first.X, first.Y + 2, first.Z);
			for (int i = 0; i < 20; i++)
			{
				client.SendMcpeMovePlayer();
				Thread.Sleep(500);
			}

			Log.Warn($"Inspect: {client.Chunks.Count} chunks received, standing at {client.CurrentLocation}");

			foreach (BlockCoordinates coordinates in wanted)
			{
				var chunkCoordinates = new ChunkCoordinates(coordinates.X >> 4, coordinates.Z >> 4);
				// A chunk that failed to decode is still put in the dictionary, as null.
				if (!client.Chunks.TryGetValue(chunkCoordinates, out ChunkColumn chunk) || chunk == null)
				{
					Log.Warn($"Inspect {coordinates}: chunk {chunkCoordinates} never arrived or did not decode");
					continue;
				}

				int runtimeId = chunk.GetBlockRuntimeId(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f);
				Block block = chunk.GetBlockObject(coordinates.X & 0x0f, coordinates.Y, coordinates.Z & 0x0f);
				bool hasBlockEntity = chunk.GetBlockEntity(coordinates) != null;

				Log.Warn($"Inspect {coordinates}: runtimeId={runtimeId} block={block?.Name ?? "(null)"} blockEntity={hasBlockEntity}");
			}
		}

		/// <summary>Right-clicks a block the way a server-authoritative client does: the use-item
		/// transaction folded into PlayerAuthInput, not a standalone McpeInventoryTransaction. This is
		/// the only way to exercise that path without a real client.</summary>
		private static void InteractWithBlock(MiNetClient client, string spec)
		{
			string[] parts = spec.Split(',');
			var target = new BlockCoordinates(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));

			// Stand next to it, looking at it.
			var position = new Vector3(target.X + 0.5f, target.Y + 1, target.Z + 2.5f);
			client.CurrentLocation = new PlayerLocation(position, 0, 0, 20);

			Log.Warn($"Interact: standing at {position}, right-clicking {target}");

			Vector3 previous = position;
			for (int i = 0; i < 80; i++)
			{
				var input = McpePlayerAuthInput.CreateObject();
				input.playerRotation = new Vector2(20, 0);
				input.playerHeadRotation = 0;
				input.position = position;
				input.moveVector = Vector2.Zero;
				input.inputData = 0;
				input.inputMode = McpePlayerAuthInput.InputMode.Mouse;
				input.playMode = McpePlayerAuthInput.ClientPlayMode.Normal;
				input.newInteractionModel = McpePlayerAuthInput.NewInteractionModel.Touch;
				input.interactRotation = new Vector2(20, 0);
				input.clientTick = i;
				input.posDelta = position - previous;
				input.analogMoveVector = Vector2.Zero;
				input.cameraOrientation = new Vector3(20, 0, 0);
				input.rawMoveVector = Vector2.Zero;

				// Give the server time to send the chunks first, then click once.
				if (i == 60)
				{
					input.itemUseTransaction = new PackedItemUseLegacyInventoryTransaction
					{
						itemUseTransaction = new ItemUseInventoryTransaction
						{
							actionType = ItemUseInventoryTransaction.ItemUseActionType.Place,
							triggerType = ItemUseInventoryTransaction.ItemUseTriggerType.PlayerInput,
							position = target,
							face = (byte) BlockFace.Up,
							slot = 0,
							item = new ItemAir(),
							fromPosition = position,
							clickPosition = new Vector3(0.5f, 1.0f, 0.5f),
							targetBlockId = 0,
							clientInteractPrediction = ItemUseInventoryTransaction.ItemUsePredictedResult.Success,
							clientCooldownState = ItemUseInventoryTransaction.ItemUseClientCooldownState.Off
						}
					};

					Log.Warn($"Interact: sending the use-item transaction inside auth input for {target}");
				}

				client.SendPacket(input);
				Thread.Sleep(50);
			}

			Log.Warn("Interact: sent, waiting for the server to answer");
			Thread.Sleep(5000);
			Log.Warn("Interact: done");
		}

		static void Main(string[] args)
		{
			// Both repositories, deliberately: log4net repositories are per assembly, so configuring
			// only the entry assembly leaves every logger inside MiNET.dll (the whole NetherNet and
			// Rtc stack) writing into an unconfigured repository, which is silence.
			var logConfig = new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.xml"));
			XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), logConfig);
			XmlConfigurator.Configure(LogManager.GetRepository(typeof(MiNetServer).Assembly), logConfig);

			Log.Info(MiNET);
			Console.WriteLine(MiNET);
			Console.WriteLine("Starting client...");

			// Username from the environment so several bots can be on a server at once. Questions
			// about who appears in a player list, and how many records each join produces, cannot be
			// answered with a single connection: with one player "everyone" and "everyone but me"
			// are the same list.
			string username = Environment.GetEnvironmentVariable("MINET_USERNAME") ?? "TheGrey";

			// MINET_TARGET=host:port points the bot somewhere other than the local BDS, e.g. at
			// MiNET itself or at MiNET.Tunnel.
			string targetEnv = Environment.GetEnvironmentVariable("MINET_TARGET");
			IPEndPoint target = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19132);
			if (!string.IsNullOrWhiteSpace(targetEnv))
			{
				string[] parts = targetEnv.Split(':');
				IPAddress ip = IPAddress.TryParse(parts[0], out var parsed) ? parsed : Dns.GetHostAddresses(parts[0])[0];
				target = new IPEndPoint(ip, parts.Length > 1 ? int.Parse(parts[1]) : 19132);
			}

			var client = new MiNetClient(target, username);

			// MINET_RADIUS overrides the view distance this bot asks for. Large radii are the
			// interesting case: one ClientCacheBlobStatus carries at most 4095 ids, so a join wide
			// enough to announce more than that is where the client's own flushing has to hold up.
			if (int.TryParse(Environment.GetEnvironmentVariable("MINET_RADIUS"), out int radius) && radius > 0) client.ChunkRadius = radius;
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("test.pmmp.io").AddressList[0], 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.4"), 19162), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("213.89.103.206"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			client.MessageHandler = new BedrockTraceHandler(client);
			// The bot announces ClientCacheStatus enabled, the way every real client does, and both
			// servers then serve blob-addressed chunks. MINET_BLOB_CACHE=0 says no instead, which
			// puts BDS back on the plain inline flow; MiNET refuses a client that says it.
			client.UseBlobCache = Environment.GetEnvironmentVariable("MINET_BLOB_CACHE") != "0";

			// MINET_XBL=1 logs in with a real Xbox Live account instead of an offline identity, which
			// is what an online-mode server requires. The first run prints a code to enter in a
			// browser; after that the saved refresh token is used and nothing is asked.
			// MINET_XBL=signaling uses the account only for the NetherNet signaling assertion
			// (mandatory on BDS 1.26.50) and still logs in offline under the bot's own name, so the
			// bot can share a server with the account's real client session.
			// MINET_XUID makes the offline login chain claim an xuid instead of the empty one a real
			// offline player has. Only useful to get the bot named in a server's permissions file.
			string offlineXuid = Environment.GetEnvironmentVariable("MINET_XUID");
			if (!string.IsNullOrWhiteSpace(offlineXuid)) client.OfflineXuid = offlineXuid.Trim();

			string xblMode = Environment.GetEnvironmentVariable("MINET_XBL");
			if (xblMode == "1" || xblMode == "signaling")
			{
				var authentication = new XboxAuthentication();
				authentication.DeviceCodeRequired += (uri, code) =>
				{
					Console.WriteLine();
					Console.WriteLine("=======================================================");
					Console.WriteLine($"  Sign in at : {uri}");
					Console.WriteLine($"  Enter code : {code}");
					Console.WriteLine("=======================================================");
					Console.WriteLine();
				};

				client.XboxIdentity = authentication.AuthenticateAsync().GetAwaiter().GetResult();
				client.SignalingAuthOnly = xblMode == "signaling";
				Console.WriteLine($"Authenticated as {client.XboxIdentity.DisplayName}{(client.SignalingAuthOnly ? " (signaling only, offline login)" : "")}");
			}

			// There is no offline ping and no connection handshake on NetherNet: signaling is one
			// HTTP round trip and the data channel opening is the connection.
			Console.WriteLine($"Connecting over NetherNet to {client.ServerEndPoint} ...");

			if (!client.ConnectNetherNetAsync().GetAwaiter().GetResult())
			{
				Console.WriteLine("Failed to connect over NetherNet");
				return;
			}

			Console.WriteLine("NetherNet data channel open, logging in ...");

			Console.WriteLine("Waiting for spawn...");
			client.PlayerStatusChangedWaitHandle.WaitOne();
			Console.WriteLine("... spawned");

			ReportChunkCache(client);

			// MINET_WALK=<blocks> walks the bot that far in +X and reports again. The server serves
			// the spawn burst as skeletons and switches to pushed columns for the rim once a player
			// is moving, so standing still shows only half of what a client has to handle.
			string walk = Environment.GetEnvironmentVariable("MINET_WALK");
			if (!string.IsNullOrWhiteSpace(walk) && int.TryParse(walk, out int walkBlocks))
			{
				WalkForward(client, walkBlocks);
				ReportChunkCache(client);
			}

			client.HasSpawned = true;

			if (Environment.GetEnvironmentVariable("MINET_EMULATE") == "1")
			{
				RealClientEmulator.Run(client);
			}

			// MINET_BLOCK_PROBE=1 has the server place one of every container block next to the bot
			// and leaves what the server answers with in the log. The block entity data that comes
			// back carries the savegame id of each one, which is the only way to know what vanilla
			// calls a barrel's or a copper chest's tile rather than inferring it from convention.
			if (Environment.GetEnvironmentVariable("MINET_BLOCK_PROBE") == "1")
			{
				ProbeContainerBlocks(client);
			}

			// MINET_STATE_PROBE=<path to mojang-blocks.json> derives every block's real integer state
			// ranges from the server itself, because no shipped file carries them: the module declares
			// one domain per state NAME, which is the union over every block using it. Writes
			// MINET_STATE_PROBE_OUT (default temp_auto/state-ranges.json).
			string stateProbe = Environment.GetEnvironmentVariable("MINET_STATE_PROBE");
			if (!string.IsNullOrWhiteSpace(stateProbe))
			{
				// Two below the bot, so the probe's own blocks never land where it stands.
				var scratch = (BlockCoordinates) client.CurrentLocation;
				scratch.Y -= 2;

				new StateRangeProber(client, scratch).Run(
					stateProbe,
					Environment.GetEnvironmentVariable("MINET_STATE_PROBE_OUT") ?? "temp_auto/state-ranges.json");
			}

			// MINET_INSPECT="x,y,z;x,y,z" walks the bot to the first coordinate and reports what the
			// server sent for each: the runtime id, the block it decodes to, and whether a block entity
			// came with it. That is the only view of a ghost block that is neither the world file nor
			// the server's own memory.
			string inspect = Environment.GetEnvironmentVariable("MINET_INSPECT");
			if (!string.IsNullOrWhiteSpace(inspect))
			{
				InspectBlocks(client, inspect);
			}

			// MINET_INTERACT="x,y,z" right-clicks that block through the auth-input path.
			string interact = Environment.GetEnvironmentVariable("MINET_INTERACT");
			if (!string.IsNullOrWhiteSpace(interact))
			{
				InteractWithBlock(client, interact);
			}

			Action<Task, PlayerLocation> doMoveTo = BotHelpers.DoMoveTo(client);

			Action<Task, string> doSendCommand = BotHelpers.DoSendCommand(client);

			Task.Run(BotHelpers.DoWaitForSpawn(client))
				// Bot commands disabled: McpeCommandRequest is not updated to the current
				// protocol yet, so keep un-modernized packets off the wire.
				//.ContinueWith(t => doSendCommand(t, $"/me says \"I spawned at {client.CurrentLocation}\""))
				//.ContinueWith(task =>
				//{
				//	var request = new McpeCommandRequest();
				//	request.command = "/setblock ~ ~-1 ~ log 0 replace";
				//	request.unknownUuid = new UUID(Guid.NewGuid().ToString());
				//	client.SendPacket(request);

				//	var coord =  (BlockCoordinates) client.CurrentLocation;
				//	var pick = McpeBlockPickRequest.CreateObject();
				//	pick.x = coord.X;
				//	pick.y = coord.Y;
				//	pick.z = coord.Z;
				//	client.SendPacket(request);
				//})

				//.ContinueWith(t => BotHelpers.DoMobEquipment(client)(t, new ItemBlock(new Cobblestone()) {Count = 64}, 0))
				//.ContinueWith(t => BotHelpers.DoMoveTo(client)(t, new PlayerLocation(client.CurrentLocation.ToVector3() - new Vector3(0, 1, 0), 180, 180, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(40, 5.62f, -20, 180, 180, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(22, 5.62, 40, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180)))
				//.ContinueWith(t => doSendCommand(t, "/me says \"Hi guys! It is I!!\""))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/summon sheep"))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/kill @e[type=sheep]"))
				.ContinueWith(t => Task.Delay(5000).Wait())
				// Skin switching test: sends a recoloured skin at the given frames per second.
				//.ContinueWith(t => BotHelpers.DoCycleSkinColors(client)(t, 20))
				//.ContinueWith(t =>
				//{
				//	Random rnd = new Random();
				//	while (true)
				//	{
				//		doMoveTo(t, new PlayerLocation(rnd.Next(10, 40), 5.62f, rnd.Next(-50, -10), 180, 180, 180));
				//		//doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180));
				//		doMoveTo(t, new PlayerLocation(rnd.Next(40, 50), 5.62f, rnd.Next(0, 20), 180, 180, 180));
				//	}
				//})
				;

			//string fileName = Path.GetTempPath() + "MobSpawns_" + Guid.NewGuid() + ".txt";
			//FileStream file = File.OpenWrite(fileName);
			//Log.Info($"Writing mob spawns to file:\n{fileName}");
			//_mobWriter = new IndentedTextWriter(new StreamWriter(file));
			//Task.Run(BotHelpers.DoWaitForSpawn(client))
			//	.ContinueWith(task =>
			//	{
			//		foreach (EntityType entityType in Enum.GetValues(typeof(EntityType)))
			//		{
			//			if (entityType == EntityType.Wither) continue;
			//			if (entityType == EntityType.Dragon) continue;
			//			if (entityType == EntityType.Slime) continue;

			//			string entityName = entityType.ToString();
			//			entityName = Regex.Replace(entityName, "([A-Z])", "_$1").TrimStart('_').ToLower();
			//			{
			//				string command = $"/summon {entityName}";
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = command;
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}

			//			Task.Delay(500).Wait();

			//			{
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = $"/kill @e[type={entityName}]";
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}
			//		}

			//		{
			//			McpeCommandRequest request = new McpeCommandRequest();
			//			request.command = $"/kill @e[type=!player]";
			//			request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//			client.SendPackage(request);
			//		}

			//	});

			using var stopped = new ManualResetEventSlim();
			int shuttingDown = 0;

			// The same leave the client does on <Enter>, reachable from a signal so a run with no
			// console still says goodbye: closing the session is the goodbye on NetherNet (the
			// transport teardown tells the server), where an outright kill costs the server its
			// inactivity timeout instead. Idempotent because several of the hooks below can fire
			// for one stop.
			void Shutdown()
			{
				if (Interlocked.Exchange(ref shuttingDown, 1) != 0) return;

				client.StopClient();
				stopped.Set();
			}

			// Cancel the signal so the runtime lets us finish rather than tearing the process down
			// mid-disconnect. Nothing can hook an outright kill, so that path still costs a timeout.
			Console.CancelKeyPress += (_, e) =>
			{
				e.Cancel = true;
				Shutdown();
			};

			using var sigterm = PosixSignalRegistration.Create(PosixSignal.SIGTERM, context =>
			{
				context.Cancel = true;
				Shutdown();
			});

			using var sigint = PosixSignalRegistration.Create(PosixSignal.SIGINT, context =>
			{
				context.Cancel = true;
				Shutdown();
			});

			AppDomain.CurrentDomain.ProcessExit += (_, _) => Shutdown();

			// Started without a console (background process, service, redirected stdin) ReadLine
			// returns immediately on EOF and the bot would quit the moment it finished logging in.
			if (Console.IsInputRedirected)
			{
				Console.WriteLine("Running until terminated.");
				stopped.Wait();
			}
			else
			{
				Console.WriteLine("<Enter> to exit!");
				Console.ReadLine();
				Shutdown();
			}
		}
	}
}