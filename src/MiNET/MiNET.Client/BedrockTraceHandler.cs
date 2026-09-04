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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.Client
{
	public class BedrockTraceHandler : McpeClientMessageHandlerBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockTraceHandler));


		public BedrockTraceHandler(MiNetClient client) : base(client)
		{
		}

		public override void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message)
		{
			Log.Warn($"Got soft enum update for {message}");
		}

		private static int _subChunkPacketsLogged;

		// Positional-id extraction (block-order pipeline): in non-hash mode the palette values in
		// subchunk storages ARE indexes into BDS's canonical block palette. Dump world coord ->
		// positional id for the placement region (chunks 0..3, see temp_auto placer.js); joined
		// with the placement layout this yields the canonical order. Lines: x\ty\tz\tid.
		private static readonly object _positionalDumpLock = new object();
		private static StreamWriter _positionalDump;
		private static int _positionalCells;

		private void DumpPositionalIds(McpeSubChunkPacket message, SubChunkPacketData entry)
		{
			int chunkX = message.centerPos.subchunkPositionX + entry.subchunkPosOffset.subchunkOffsetX;
			int chunkZ = message.centerPos.subchunkPositionZ + entry.subchunkPosOffset.subchunkOffsetZ;
			if (chunkX < 0 || chunkX > 3 || chunkZ < 0 || chunkZ > 3) return;

			int[] grid = ClientUtils.DecodeSubChunkGrid(entry.serializedSubChunk);
			if (grid == null) return;

			int baseX = chunkX * 16;
			int baseY = (message.centerPos.subchunkPositionY + entry.subchunkPosOffset.subchunkOffsetY) * 16;
			int baseZ = chunkZ * 16;
			lock (_positionalDumpLock)
			{
				_positionalDump ??= new StreamWriter(@"c:\Development\github\MiNET\temp_auto\positional-ids.txt", false);
				for (int i = 0; i < 4096; i++)
				{
					int x = baseX + ((i >> 8) & 15), z = baseZ + ((i >> 4) & 15), y = baseY + (i & 15);
					_positionalDump.WriteLine($"{x}\t{y}\t{z}\t{grid[i]}");
					_positionalCells++;
				}
				_positionalDump.Flush();
			}
		}

		public override void HandleMcpeSubChunkPacket(McpeSubChunkPacket message)
		{
			// The verdicts and the section payloads belong to the client's chunk cache, which holds
			// what arrives instead of answering blind misses for bytes it then drops.
			base.HandleMcpeSubChunkPacket(message);

			// Everything below is trace/verification work, and at fleet scale it is what melts the
			// emulator process (measured ~14 cores and 0.9 s/s GC pause on a 200-bot join burst).
			if (Client.IsEmulator) return;

			int success = 0, allAir = 0, other = 0, parsedOk = 0, parseFail = 0, cached = 0;
			foreach (SubChunkPacketData entry in message.subchunkData)
			{
				switch ((SubChunkPacketData.SubchunkRequestResult) entry.subchunkRequestResult)
				{
					case SubChunkPacketData.SubchunkRequestResult.Success:
						success++;

						// Cached, the section is a hash and its bytes arrive later in a
						// ClientCacheMissResponse; serializedSubChunk carries the block entities
						// instead, so there is nothing here to parse. MINET_BLOB_CACHE=0 puts the
						// payload back in the packet, which is the mode this check is for.
						if (message.cacheEnabled)
						{
							cached++;
							break;
						}

						if (ClientUtils.TryParseSubChunkPayload(entry.serializedSubChunk, Client.BlockNetworkIdsAreHashes)) parsedOk++;
						else parseFail++;
						if (!Client.BlockNetworkIdsAreHashes) DumpPositionalIds(message, entry);
						break;
					case SubChunkPacketData.SubchunkRequestResult.Successallair:
						allAir++;
						break;
					default:
						other++;
						break;
				}
			}

			if (System.Threading.Interlocked.Increment(ref _subChunkPacketsLogged) <= 5 || parseFail > 0)
			{
				Log.Warn($"SubChunk response: origin=({message.centerPos.subchunkPositionX},{message.centerPos.subchunkPositionY},{message.centerPos.subchunkPositionZ}) entries={message.subchunkData.Count} success={success} cached={cached} parsedOk={parsedOk} parseFail={parseFail} allAir={allAir} other={other} positionalCells={_positionalCells}");
			}
		}

		public override void HandleMcpeDisconnect(McpeDisconnect message)
		{
			Log.Warn($"Disconnect {Client.Username}: reason={message.reason} message={message.message}");

			base.HandleMcpeDisconnect(message);
		}

		public override void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message)
		{
			var sb = new StringBuilder();
			sb.AppendLine();

			sb.AppendLine("Resource packs:");
			foreach (PackInfoData info in message.resourcePacks)
			{
				sb.AppendLine($"ID={info.packIdVersion.packUuid}, Version={info.packIdVersion.packVersion}, Size={info.packSize}");
			}

			Log.Debug(sb.ToString());

			base.HandleMcpeResourcePacksInfo(message);
		}

		public override void HandleMcpeResourcePackStack(McpeResourcePackStack message)
		{
			//Log.Debug($"HEX: \n{Package.HexDump(message.Bytes)}");

			var sb = new StringBuilder();
			sb.AppendLine();

			sb.AppendLine("Resource pack stacks:");
			foreach (var info in message.resourcepackidversions)
			{
				sb.AppendLine($"ID={info.Id}, Version={info.Version}, Subpackname={info.SubPackName}");
			}

			Log.Debug(sb.ToString());

			base.HandleMcpeResourcePackStack(message);
		}

		//private bool _runningBlockMetadataDiscovery;

		private List<ICommandExecutioner> _executioners = new List<ICommandExecutioner>() {new PlaceAllBlocksExecutioner()};

		private void CallPacketHandlers(Packet packet)
		{
			var wantExec = _executioners.Where(e => e is IGenericPacketHandler);
			List<Task> tasks = new List<Task>();
			foreach (var commandExecutioner in wantExec)
			{
				var executioner = (IGenericPacketHandler) commandExecutioner;
				tasks.Add(Task.Run(() => executioner.HandlePacket(this, packet)));
			}
			Task.WaitAll(tasks.ToArray());
		}

		public override void HandleMcpeText(McpeText message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Text: {message.message}");

			string text = message.message;
			if (string.IsNullOrEmpty(text)) return;

			var wantExec = _executioners.Where(e => e.CanExecute(text));

			foreach (var executioner in wantExec)
			{
				Log.Debug($"Executing command handler: {executioner.GetType().FullName}");
				Task.Run(() => executioner.Execute(this, text));
			}
		}

		public override void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
			Log.Debug($"Inventory slot: {message.item}");
		}

		public override void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
			CallPacketHandlers(message);
		}

		public override void HandleMcpeItemStackResponse(McpeItemStackResponse message)
		{
			foreach (ItemStackResponseInfo response in message.responses)
			{
				Log.Warn($"SPLIT RESPONSE: request {response.clientRequestId} -> {response.result}");
				if (response.containers == null) continue;

				foreach (ItemStackResponseContainerInfo container in response.containers)
				foreach (ItemStackResponseSlotInfo slot in container.slots)
				{
					Log.Warn($"SPLIT RESPONSE:   container {container.fullContainerName.containerName} slot {slot.requestedSlot} count {slot.amount} stackNetId {slot.itemStackNetId}");
				}
			}
		}

		private bool _splitSent;

		/// <summary>A request slot, named by container and slot, citing the stack id we believe is there.</summary>
		private static ItemStackRequestSlotInfo Slot(int container, byte slot, int stackNetId)
		{
			return new ItemStackRequestSlotInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) container},
				slot = slot,
				netIdVariant = stackNetId
			};
		}

		public override void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
			CallPacketHandlers(message);

			Log.Debug($"Set container content on Window ID: 0x{message.inventoryId:x2}, Count: {message.input.Count}");

			// SPLIT TEST: on the first stack of more than one, ask the server to move half of it into
			// an empty slot, then read the ids the response comes back with.
			if (Environment.GetEnvironmentVariable("MINET_SPLIT_TEST") == "1" && !_splitSent && message.inventoryId == 0)
			{
				for (int slot = 0; slot < message.input.Count; slot++)
				{
					Item item = message.input[slot];
					if (item == null || item.Count < 2) continue;

					_splitSent = true;
					Log.Warn($"SPLIT: source slot {slot} holds {item.Name} x{item.Count}, stack net id {item.UniqueId}");

					int half = item.Count / 2;
					int target = slot + 1;

					var packet = McpeItemStackRequest.CreateObject();
					packet.requests = new List<ItemStackRequest>();
					var actions = new ItemStackRequest
					{
						clientRequestId = -1,
						actions = new List<ItemStackRequestBase>
						{
							new ItemStackRequestTakeAction
							{
								amount = (byte) half,
								source = Slot(28, (byte) slot, item.UniqueId),
								destination = Slot(28, (byte) target, 0)
							}
						}
					};
					packet.requests.Add(actions);
					Log.Warn($"SPLIT: taking {half} from slot {slot} into slot {target}");
					Client.SendPacket(packet);
					break;
				}
			}

			if (Client.IsEmulator) return;

			ItemStacks slots = message.input;

			//if (message.inventoryId == 0x79)
			//{
			//	string fileName = Path.GetTempPath() + "Inventory_0x79_" + Guid.NewGuid() + ".txt";
			//	Client.WriteInventoryToFile(fileName, slots);
			//}
			//else if (message.inventoryId == 0x00)
			//{
			//	//string fileName = Path.GetTempPath() + "Inventory_0x00_" + Guid.NewGuid() + ".txt";
			//	//Client.WriteInventoryToFile(fileName, slots);
			//}
		}

		public override void HandleMcpeCreativeContent(McpeCreativeContent message)
		{
			ItemStacks slots = new ItemStacks();
			foreach (var entry in message.entries)
			{
				slots.Add(entry.itemInstance);
			}

			// Off the session thread; blocking here for seconds makes the server
			// drop the connection during the join sequence.
			string fileName = Path.GetTempPath() + "Inventory_0x79_" + Guid.NewGuid() + ".txt";
			Task.Run(() => Client.WriteInventoryToFile(fileName, slots));
		}

		public override void HandleMcpeAddItemEntity(McpeAddItemEntity message)
		{
			CallPacketHandlers(message);
		}

		// Live positional-id capture. In non-hash mode the runtime id on a block-change packet is
		// the canonical palette index. Bedrock broadcasts the change in the SAME tick as the
		// placement, before the queued shape update recomputes connection states, so the FIRST id
		// seen for a coordinate is the state that was actually asked for. Later packets for that
		// coordinate are the recompute and must not overwrite it, hence first-write-wins.
		private static readonly object _liveIdLock = new object();
		private static readonly Dictionary<BlockCoordinates, uint> _liveIds = new Dictionary<BlockCoordinates, uint>();
		private static StreamWriter _liveIdDump;

		private void RecordLiveId(BlockCoordinates coord, uint runtimeId)
		{
			if (Client.BlockNetworkIdsAreHashes) return;
			lock (_liveIdLock)
			{
				if (!_liveIds.TryAdd(coord, runtimeId)) return;
				_liveIdDump ??= new StreamWriter(@"c:\Development\github\MiNET\temp_auto\live-ids.txt", false) {AutoFlush = true};
				_liveIdDump.WriteLine($"{coord.X}\t{coord.Y}\t{coord.Z}\t{runtimeId}");
				if (_liveIds.Count % 2000 == 0) Log.Warn($"Live positional ids captured: {_liveIds.Count}");
			}
		}

		public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
			if (message.storage == 0) RecordLiveId(message.coordinates, message.blockRuntimeId);
			CallPacketHandlers(message);
		}

		public override void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{
			// Batched changes: coordinates on the entries are absolute, same as McpeUpdateBlock.
			if (message.layerZeroUpdates == null) return;
			foreach (UpdateSubChunkBlocksPacketEntry entry in message.layerZeroUpdates)
			{
				RecordLiveId(entry.Coordinates, entry.BlockRuntimeId);
			}
		}

		public override void HandleMcpeStartGame(McpeStartGame message)
		{
			// Identity, spawn, the chunk-radius request and the loading screen are protocol
			// behaviour and live in the base handler, which asks for the radius this client was
			// configured with. This override only reports what arrived.
			base.HandleMcpeStartGame(message);

			Log.Warn($"Got position from startgame packet: {Client.CurrentLocation}");
			Log.Warn($"StartGame: blockNetworkIdsAreHashes={message.blockNetworkIdsAreHashes}, position={message.position}");
			Client.BlockNetworkIdsAreHashes = message.blockNetworkIdsAreHashes;

			// Verify the server's block registry checksum like the real client does: compute from
			// our own palette and compare. 0 from the server means "no claim" (MiNET, PMMP).
			// Against BDS this is the live known-answer test for the checksum algorithm; MISMATCH
			// is expected until the algorithm is cracked (see NetworkBlockPalette).
			if (message.serverBlockTypeRegistryChecksum != 0)
			{
				ulong computed = NetworkBlockPalette.ComputeRegistryChecksum();
				bool match = computed == message.serverBlockTypeRegistryChecksum;
				Log.Warn($"Registry checksum: received={message.serverBlockTypeRegistryChecksum} computed={computed} => {(match ? "MATCH, algorithm verified" : "MISMATCH, algorithm candidate wrong")}");
			}
			else
			{
				Log.Warn("Registry checksum: server sent 0 (no claim), nothing to verify");
			}

			if (message.blockProperties != null && message.blockProperties.Count > 0)
			{
				Log.Warn($"StartGame carries {message.blockProperties.Count} custom block properties");
			}

			LogGamerules(message.settings.gamerules);
		}

		public override void HandleMcpeAddPlayer(McpeAddPlayer message)
		{
			if (Client.IsEmulator) return;

			Log.DebugFormat("McpeAddPlayer Unique ID: {0}", message.abilitiesData?.targetPlayerRawId);
			Log.DebugFormat("McpeAddPlayer Runtime Entity ID: {0}", message.runtimeEntityId);
			Log.DebugFormat("Position: {0}", message.position);
			Log.DebugFormat("Rotation: {0}", message.rotation);
			Log.DebugFormat("Head rotation: {0}", message.yHeadRotation);
			Log.DebugFormat("Velocity: {0}", message.velocity);
			Log.DebugFormat("Metadata: {0}", Client.MetadataToCode(message.metadata));
			Log.DebugFormat("Links count: {0}", message.links?.Count);
		}

		public override void HandleMcpeAddEntity(McpeAddEntity message)
		{
			if (Client.IsEmulator) return;

			if (!Client.Entities.ContainsKey(message.entityIdSelf))
			{
				var entity = new Entity(message.entityType, null);
				entity.EntityId = message.runtimeEntityId;
				entity.KnownPosition = new PlayerLocation(message.position.X, message.position.Y, message.position.Z, message.rotation.Y, message.rotation.Y, message.rotation.X);
				entity.Velocity = message.velocity;
				Client.Entities.TryAdd(entity.EntityId, entity);
			}

			Log.DebugFormat("McpeAddEntity Entity ID: {0}", message.entityIdSelf);
			Log.DebugFormat("McpeAddEntity Runtime Entity ID: {0}", message.runtimeEntityId);
			Log.DebugFormat("Entity Type: {0}", message.entityType);
			Log.DebugFormat("Position: {0}", message.position);
			Log.DebugFormat("Rotation: {0}", message.rotation);
			Log.DebugFormat("Velocity: {0}", message.velocity);
			Log.DebugFormat("Metadata: {0}", Client.MetadataToCode(message.metadata));
			Log.DebugFormat("Links count: {0}", message.links?.Count);

			if (message.metadata.Contains(0))
			{
				long? value = ((MetadataLong) message.metadata[0])?.Value;
				if (value != null)
				{
					long dataValue = (long) value;
					Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString(dataValue, 2)}b ");
				}
			}

			if (Log.IsDebugEnabled)
			{
				foreach (var attribute in message.attributes)
				{
					Log.Debug($"Entity attribute {attribute}");
				}
			}

			Log.DebugFormat("Links count: {0}", message.links);

			if (Log.IsDebugEnabled && Client._mobWriter != null)
			{
				Client._mobWriter.WriteLine("Entity Type: {0}", message.entityType);
				Client._mobWriter.Indent++;
				Client._mobWriter.WriteLine("Metadata: {0}", Client.MetadataToCode(message.metadata));
				Client._mobWriter.Indent--;
				Client._mobWriter.WriteLine();
				Client._mobWriter.Flush();
			}

			if (message.entityType == "minecraft:horse")
			{
				var id = message.runtimeEntityId;
				Vector3 pos = message.position;
				Task.Run(BotHelpers.DoWaitForSpawn(Client))
					.ContinueWith(t => Task.Delay(3000).Wait())
					//.ContinueWith(task =>
					//{
					//	Log.Warn("Sending jump for player");

					//	McpeInteract action = McpeInteract.CreateObject();
					//	action.targetRuntimeEntityId = id;
					//	action.actionId = (int) 3;
					//	SendPackage(action);
					//})
					//.ContinueWith(t => Task.Delay(2000).Wait())
					//.ContinueWith(task =>
					//{
					//	for (int i = 0; i < 10; i++)
					//	{
					//		Log.Warn("Mounting horse");

					//		McpeInventoryTransaction transaction = McpeInventoryTransaction.CreateObject();
					//		transaction.transaction = new Transaction()
					//		{
					//			TransactionType = McpeInventoryTransaction.TransactionType.ItemUseOnEntity,
					//			TransactionRecords = new List<TransactionRecord>(),
					//			EntityId = id,
					//			ActionType = 0,
					//			Slot = 0,
					//			Item = new ItemAir(),
					//			//Item = new ItemBlock(new Cobblestone()) { Count = 64 },
					//			Position = BlockCoordinates.Zero,
					//			FromPosition = CurrentLocation,
					//			ClickPosition = pos,
					//		};

					//		SendPackage(transaction);
					//		Thread.Sleep(4000);
					//	}

					//})
					.ContinueWith(task =>
					{
						Log.Warn("Sending sneak for player");

						McpePlayerAction action = McpePlayerAction.CreateObject();
						action.runtimeEntityId = Client.EntityId;
						action.actionId = (int) PlayerAction.StartSneak;
						Client.SendPacket(action);
					})
					.ContinueWith(t => Task.Delay(2000).Wait())
					.ContinueWith(task =>
					{
						Log.Warn("Sending transaction for horse");

						var transaction = McpeInventoryTransaction.CreateObject();
						transaction.transaction = new ItemUseOnActorInventoryTransaction
						{
							actions = new List<InventoryAction>(),
							runtimeId = id,
							actionType = ItemUseOnActorInventoryTransaction.ItemUseOnActorActionType.Interact,
							slot = 0,
							item = new ItemAir(),
							fromPosition = Client.CurrentLocation,
							hitPosition = pos,
						};

						Client.SendPacket(transaction);
					});
			}
		}

		public override void HandleMcpeRemoveEntity(McpeRemoveEntity message)
		{
			Client.Entities.TryRemove(message.entityIdSelf, out _);
		}

		public override void HandleMcpeLevelEvent(McpeLevelEvent message)
		{
			int data = message.data;
			if (message.eventId == 2001)
			{
				int blockId = data & 0xff;
				int metadata = data >> 12;
				Log.Debug($"BlockID={blockId}, Metadata={metadata}");
			}
		}

		public override void HandleMcpeUpdateAttributes(McpeUpdateAttributes message)
		{
			foreach (var playerAttribute in message.attributes)
			{
				Log.Debug($"Attribute {playerAttribute}");
			}
		}

		public override void HandleMcpeCraftingData(McpeCraftingData message)
		{
			if (Client.IsEmulator) return;

			// Off the session thread; synchronously dumping thousands of recipes stalls
			// the join sequence long enough for the server to drop the connection.
			Recipes recipes = message.recipes;
			Task.Run(() => DumpRecipes(recipes));
		}

		private static void DumpRecipes(Recipes recipes)
		{
			string fileName = Path.GetTempPath() + "Recipes_" + Guid.NewGuid() + ".txt";
			Log.Info("Writing recipes to filename: " + fileName);
			FileStream file = File.OpenWrite(fileName);

			var writer = new IndentedTextWriter(new StreamWriter(file), "\t");

			writer.WriteLine();
			writer.Indent++;
			writer.Indent++;

			writer.WriteLine("static RecipeManager()");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine("Recipes = new Recipes");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (Recipe recipe in recipes)
			{
				var shapelessRecipe = recipe as ShapelessRecipe;
				if (shapelessRecipe != null)
				{
					writer.WriteLine($"new ShapelessRecipe(");
					writer.Indent++;

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (var itemStack in shapelessRecipe.Result)
					{
						writer.WriteLine($"new Item(\"{itemStack.Name}\", {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (var itemStack in shapelessRecipe.Input)
					{
						writer.WriteLine($"new Item(\"{itemStack.Name}\", {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapelessRecipe.Block}\"){{ UniqueId = {shapelessRecipe.UniqueId} }},");

					writer.Indent--;
					continue;
				}

				var shapedRecipe = recipe as ShapedRecipe;
				//if (shapedRecipe != null && Client._recipeToSend == null)
				//{
				//	if (shapedRecipe.Result.Id == 5 && shapedRecipe.Result.Count == 4 && shapedRecipe.Result.Metadata == 0)
				//	{
				//		Log.Error("Setting recipe! " + shapedRecipe.Id);
				//		Client._recipeToSend = shapedRecipe;
				//	}
				//}

				if (shapedRecipe != null)
				{
					writer.WriteLine($"new ShapedRecipe({shapedRecipe.Width}, {shapedRecipe.Height},");
					writer.Indent++;

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Result)
					{
						writer.WriteLine($"new Item(\"{item.Name}\", {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new Item[]");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Input)
					{
						writer.WriteLine($"new Item(\"{item.Name}\", {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapedRecipe.Block}\"){{ UniqueId = {shapedRecipe.UniqueId} }},");

					writer.Indent--;

					continue;
				}

				var smeltingRecipe = recipe as SmeltingRecipe;
				if (smeltingRecipe != null)
				{
					writer.WriteLine($"new SmeltingRecipe(new Item(\"{smeltingRecipe.Result.Name}\", {smeltingRecipe.Result.Metadata}, {smeltingRecipe.Result.Count}){{ UniqueId = {smeltingRecipe.Result.UniqueId}, RuntimeId={smeltingRecipe.Result.RuntimeId} }}, new Item(\"{smeltingRecipe.Input.Name}\", {smeltingRecipe.Input.Metadata}){{ UniqueId = {smeltingRecipe.Input.UniqueId}, RuntimeId={smeltingRecipe.Input.RuntimeId} }}, \"{smeltingRecipe.Block}\"),");
					continue;
				}

				var multiRecipe = recipe as MultiRecipe;
				if (multiRecipe != null)
				{
					writer.WriteLine($"new MultiRecipe() {{ Id = new UUID(\"{recipe.Id}\"), UniqueId = {multiRecipe.UniqueId} }}, // {recipe.Id}");
					continue;
				}
			}

			writer.Indent--;
			writer.WriteLine("};");
			writer.Indent--;
			writer.WriteLine("}");

			writer.Flush();
			file.Close();
			//Environment.Exit(0);
		}

		public override void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
			Log.DebugFormat("X: {0}", message.coordinates.X);
			Log.DebugFormat("Y: {0}", message.coordinates.Y);
			Log.DebugFormat("Z: {0}", message.coordinates.Z);
			Log.DebugFormat("NBT:\n{0}", message.namedtag.NbtFile.RootTag);
		}

		public override void HandleMcpeLevelChunk(McpeLevelChunk message)
		{
			// Blob-status replies and subchunk requests are protocol behaviour and live in the
			// base handler; this override only decodes and records what arrived.
			base.HandleMcpeLevelChunk(message);

			if (Client.IsEmulator) return;

			if (message.cacheEnabled)
			{
				foreach (ulong hash in message.cacheMetadata)
				{
					Log.Debug($"Got hashes for {message.chunkPosition.x}, {message.chunkPosition.z}, {hash}");
				}
			}
			else if (message.clientRequestSubchunkLimit == null)
			{
				Client.Chunks.GetOrAdd(new ChunkCoordinates(message.chunkPosition.x, message.chunkPosition.z), coordinates =>
				{
					Log.Debug($"Chunk X={message.chunkPosition.x}, Z={message.chunkPosition.z}, size={message.chunkData.Length}, Count={Client.Chunks.Count}");

					ChunkColumn chunk = null;
					try
					{
						chunk = ClientUtils.DecodeChunkColumn((int) message.subChunkCount, message.chunkData, blockNetworkIdsAreHashes: Client.BlockNetworkIdsAreHashes);
						if (chunk != null)
						{
							chunk.X = coordinates.X;
							chunk.Z = coordinates.Z;
							chunk.RecalcHeight();
							Log.DebugFormat("Chunk X={0}, Z={1}", chunk.X, chunk.Z);
							foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntity in chunk.BlockEntities)
							{
								Log.Debug($"Blockentity: {blockEntity.Value}");
							}
						}
					}
					catch (Exception e)
					{
						Log.Error("Reading chunk", e);
					}

					return chunk;
				});
			}
		}

		public override void HandleMcpeGameRulesChanged(McpeGameRulesChanged message)
		{
			GameRules rules = message.rules;
			LogGamerules(rules);
		}

		private static void LogGamerules(GameRules rules)
		{
			foreach (var rule in rules)
			{
				if (rule is GameRule<bool>)
				{
					Log.Debug($"Rule: {rule.Name}={(GameRule<bool>) rule}");
				}
				else if (rule is GameRule<int>)
				{
					Log.Debug($"Rule: {rule.Name}={(GameRule<int>) rule}");
				}
				else if (rule is GameRule<float>)
				{
					Log.Debug($"Rule: {rule.Name}={(GameRule<float>) rule}");
				}
				else
				{
					Log.Warn($"Rule: {rule.Name}={rule}");
				}
			}
		}

		public override void HandleMcpeAvailableCommands(McpeAvailableCommands message)
		{
			//{
			//	dynamic json = JObject.Parse(message.commands);

			//	//if (Log.IsDebugEnabled) Log.Debug($"Command JSON:\n{json}");
			//	string fileName = Path.GetTempPath() + "AvailableCommands_" + Guid.NewGuid() + ".json";
			//	Log.Info($"Writing commands to filename: {fileName}");
			//	File.WriteAllText(fileName, message.commands);
			//}
			//{
			//	dynamic json = JObject.Parse(message.unknown);

			//	//if (Log.IsDebugEnabled) Log.Debug($"Command (unknown) JSON:\n{json}");
			//}
		}

		public override void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message)
		{
			string fileName = Path.GetTempPath() + "ResourcePackChunkData_" + message.packageId + ".zip";
			Log.Warn("Writing ResourcePackChunkData part " + message.chunkIndex.ToString() + " to filename: " + fileName);

			FileStream file = File.OpenWrite(fileName);
			file.Seek((long) message.progress, SeekOrigin.Begin);

			file.Write(message.payload, 0, message.payload.Length);
			file.Close();

			Log.Debug($"packageId={message.packageId}");
			Log.Debug($"unknown1={message.chunkIndex}");
			Log.Debug($"unknown3={message.progress}");
			Log.Debug($"Actual Lenght={message.payload.Length}");

			base.HandleMcpeResourcePackChunkData(message);
		}

		public override void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message)
		{
			foreach (var entity in message.namedtag.NbtFile.RootTag["idlist"] as NbtList)
			{
				var id = (entity["id"] as NbtString).Value;
				var rid = (entity["rid"] as NbtInt).Value;
				if (!Enum.IsDefined(typeof(EntityType), rid))
				{
					Log.Debug($"{{ (EntityType) {rid}, \"{id}\" }},");
				}
			}
		}

		public override void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message)
		{
			//NbtCompound list = new NbtCompound("");
			//foreach (Biome biome in Biomes)
			//{
			//	if (string.IsNullOrEmpty(biome.DefinitionName))
			//		continue;
			//	list.Add(
			//		new NbtCompound(biome.DefinitionName)
			//		{
			//			new NbtFloat("downfall", biome.Downfall),
			//			new NbtFloat("temperature", biome.Temperature),
			//		}
			//	);
			//}

			var sb = new StringBuilder();
			foreach (var entry in message.Definitions)
			{
				string name = entry.NameIndex >= 0 && entry.NameIndex < message.Strings.Count ? message.Strings[entry.NameIndex] : $"index={entry.NameIndex}";
				sb.AppendLine($"{name}: biomeId={entry.BiomeId}, temperature={entry.Temperature}, downfall={entry.Downfall}, rain={entry.Rain}");
			}
			File.WriteAllText(Path.Combine(Path.GetTempPath(), "Biomes_" + Guid.NewGuid() + ".txt"), sb.ToString());
		}

		public override void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
		{
		}

		/// <inheritdoc />
		public override void HandleMcpeCommandOutput(McpeCommandOutput message)
		{
			base.HandleMcpeCommandOutput(message);

			Client.CommandOutputReceived?.Invoke(message);
		}
	}
}






