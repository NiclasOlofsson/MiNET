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

		public override void HandleMcpeDisconnect(McpeDisconnect message)
		{
			Log.InfoFormat("Disconnect {1}: {0}", message.message, Client.Username);

			base.HandleMcpeDisconnect(message);
		}

		public override void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message)
		{
			Log.Warn($"HEX: \n{Packet.HexDump(message.Bytes)}");

			var sb = new StringBuilder();
			sb.AppendLine();

			sb.AppendLine("Texture packs:");
			foreach (TexturePackInfo info in message.texturepacks)
			{
				sb.AppendLine($"ID={info.UUID}, Version={info.Version}, Unknown={info.Size}");
			}

			sb.AppendLine("Behavior packs:");
			foreach (ResourcePackInfo info in message.behahaviorpackinfos)
			{
				sb.AppendLine($"ID={info.UUID}, Version={info.Version}");
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

			sb.AppendLine("Behavior pack stacks:");
			foreach (var info in message.behaviorpackidversions)
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

			if (text.Equals(".do"))
			{
				Client.SendCraftingEvent();
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

		public override void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
			CallPacketHandlers(message);

			Log.Debug($"Set container content on Window ID: 0x{message.inventoryId:x2}, Count: {message.input.Count}");

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
			ItemStacks slots = message.input;

			string fileName = Path.GetTempPath() + "Inventory_0x79_" + Guid.NewGuid() + ".txt";
			Client.WriteInventoryToFile(fileName, slots);
		}

		public override void HandleMcpeAddItemEntity(McpeAddItemEntity message)
		{
			CallPacketHandlers(message);
		}

		public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
		{
			CallPacketHandlers(message);
		}

		public override void HandleMcpeStartGame(McpeStartGame message)
		{
			Client.EntityId = message.runtimeEntityId;
			Client.NetworkEntityId = message.entityIdSelf;
			Client.SpawnPoint = message.spawn;
			Client.CurrentLocation = new PlayerLocation(Client.SpawnPoint, message.rotation.X, message.rotation.X, message.rotation.Y);

			BlockPalette blockPalette = message.blockPalette;
			Client.BlockPalette = message.blockPalette;

			//var blockPalette = BlockFactory.BlockStates;
			Log.Warn($"Got position from startgame packet: {Client.CurrentLocation}");

			var settings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented,
				DefaultValueHandling = DefaultValueHandling.Include
			};

			var fileNameItemstates = Path.GetTempPath() + "itemstates_" + Guid.NewGuid() + ".json";
			File.WriteAllText(fileNameItemstates, JsonConvert.SerializeObject(message.itemstates, settings));

			string fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using(FileStream file = File.OpenWrite(fileName))
			{
				var writer = new IndentedTextWriter(new StreamWriter(file));
				
				Log.Warn($"BlockPalette ({blockPalette.Count}) Filename:\n{fileName}");

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;

				var blocks = new List<(int, string)>();

				foreach (IGrouping<string, BlockStateContainer> blockstateGrouping in blockPalette.OrderBy(record => record.Name).ThenBy(record => record.Data).ThenBy(record => record.RuntimeId) .GroupBy(record => record.Name))
				{
					BlockStateContainer currentBlockState = blockstateGrouping.First();
					Log.Debug($"{currentBlockState.Name}, Id={currentBlockState.Id}");
					BlockStateContainer defaultBlockState = BlockFactory.GetBlockById(currentBlockState.Id, 0)?.GetGlobalState();
					if (defaultBlockState == null)
					{
						defaultBlockState = blockstateGrouping.FirstOrDefault(bs => bs.Data == 0);
					}

					Log.Debug($"{currentBlockState.RuntimeId}, {currentBlockState.Name}, {currentBlockState.Data}");
					Block blockById = BlockFactory.GetBlockById(currentBlockState.Id);
					bool existingBlock = blockById.GetType() != typeof(Block) && !blockById.IsGenerated;
					int id = existingBlock ? currentBlockState.Id : -1;

					string blockClassName = CodeName(currentBlockState.Name.Replace("minecraft:", ""), true);

					blocks.Add((blockById.Id, blockClassName));
					writer.WriteLineNoTabs($"");

					writer.WriteLine($"public partial class {blockClassName} // {blockById.Id} typeof={blockById.GetType().Name}");
					writer.WriteLine($"{{");
					writer.Indent++;

					writer.WriteLine($"public override string Name => \"{currentBlockState.Name}\";");
					writer.WriteLineNoTabs("");

					var bits = new List<BlockStateByte>();
					foreach (var state in blockstateGrouping.First().States)
					{
						var q = blockstateGrouping.SelectMany(c => c.States);

						// If this is on base, skip this property. We need this to implement common functionality.
						Type baseType = blockById.GetType().BaseType;
						bool propOverride = baseType != null
											&& ("Block" != baseType.Name
												&& baseType.GetProperty(CodeName(state.Name, true)) != null);

						switch (state)
						{
							case BlockStateByte blockStateByte:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateByte) d).Value).Distinct().OrderBy(s => s).ToList();
								byte defaultVal = ((BlockStateByte) defaultBlockState?.States.FirstOrDefault(s => s.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase)))?.Value ?? 0;
								if (values.Min() == 0 && values.Max() == 1)
								{
									bits.Add(blockStateByte);
									writer.Write($"[StateBit] ");
									writer.WriteLine($"public{(propOverride ? " override" : "")} bool {CodeName(state.Name, true)} {{ get; set; }} = {(defaultVal == 1 ? "true" : "false")};");
								}
								else
								{
									writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
									writer.WriteLine($"public{(propOverride ? " override" : "")} byte {CodeName(state.Name, true)} {{ get; set; }} = {defaultVal};");
								}
								break;
							}
							case BlockStateInt blockStateInt:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateInt) d).Value).Distinct().OrderBy(s => s).ToList();
								int defaultVal = ((BlockStateInt) defaultBlockState?.States.FirstOrDefault(s => s.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase)))?.Value ?? 0;
								writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
								writer.WriteLine($"public{(propOverride ? " override" : "")} int {CodeName(state.Name, true)} {{ get; set; }} = {defaultVal};");
								break;
							}
							case BlockStateString blockStateString:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateString) d).Value).Distinct().ToList();
								string defaultVal = ((BlockStateString) defaultBlockState?.States.FirstOrDefault(s => s.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase)))?.Value ?? "";
								if (values.Count > 1)
								{
									writer.WriteLine($"[StateEnum({string.Join(',', values.Select(v => $"\"{v}\""))})]");
								}
								writer.WriteLine($"public{(propOverride ? " override" : "")} string {CodeName(state.Name, true)} {{ get; set; }} = \"{defaultVal}\";");
								break;
							}
							default:
								throw new ArgumentOutOfRangeException(nameof(state));
						}
					}

					// Constructor

					//if (id == -1 || blockById.IsGenerated)
					//{
					//	writer.WriteLine($"");

					//	writer.WriteLine($"public {blockClassName}() : base({currentBlockState.Id})");
					//	writer.WriteLine($"{{");
					//	writer.Indent++;
					//	writer.WriteLine($"IsGenerated = true;");
					//	writer.WriteLine($"SetGenerated();");
					//	writer.Indent--;
					//	writer.WriteLine($"}}");
					//}

					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public override void SetState(List<IBlockState> states)");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"foreach (var state in states)");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"switch(state)");
					writer.WriteLine($"{{");
					writer.Indent++;

					foreach (var state in blockstateGrouping.First().States)
					{
						writer.WriteLine($"case {state.GetType().Name} s when s.Name == \"{state.Name}\":");
						writer.Indent++;
						writer.WriteLine($"{CodeName(state.Name, true)} = {(bits.Contains(state) ? "Convert.ToBoolean(s.Value)" : "s.Value")};");
						writer.WriteLine($"break;");
						writer.Indent--;
					}

					writer.Indent--;
					writer.WriteLine($"}} // switch");
					writer.Indent--;
					writer.WriteLine($"}} // foreach");
					writer.Indent--;
					writer.WriteLine($"}} // method");

					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public override BlockStateContainer GetState()");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"var record = new BlockStateContainer();");
					writer.WriteLine($"record.Name = \"{blockstateGrouping.First().Name}\";");
					writer.WriteLine($"record.Id = {blockstateGrouping.First().Id};");
					foreach (var state in blockstateGrouping.First().States)
					{
						string propName = CodeName(state.Name, true);
						writer.WriteLine($"record.States.Add(new {state.GetType().Name} {{Name = \"{state.Name}\", Value = {(bits.Contains(state) ? $"Convert.ToByte({propName})" : propName)}}});");
					}
					writer.WriteLine($"return record;");
					writer.Indent--;
					writer.WriteLine($"}} // method");
					writer.Indent--;
					writer.WriteLine($"}} // class");
				}

				writer.WriteLine();

				foreach (var block in blocks.OrderBy(tuple => tuple.Item1))
				{
					int clazzId = block.Item1;

					Block blockById = BlockFactory.GetBlockById(clazzId);
					bool existingBlock = blockById.GetType() != typeof(Block) && !blockById.IsGenerated;
					if (existingBlock) continue;

					string clazzName = block.Item2;
					string baseClazz = clazzName.EndsWith("Stairs") ? "BlockStairs" : "Block";
					baseClazz = clazzName.EndsWith("Slab") && !clazzName.EndsWith("DoubleSlab")? "SlabBase" : baseClazz;
					writer.WriteLine($"public partial class {clazzName} : {baseClazz} {{ " +
									$"public {clazzName}() : base({clazzId}) {{ IsGenerated = true; }} " +
									$"}}");
				}

				writer.Indent--;
				writer.WriteLine($"}}"); // namespace

				//foreach (var block in blocks.OrderBy(tuple => tuple.Item1))
				//{
				//	// 495 => new StrippedCrimsonStem(),
				//	writer.WriteLine($"\t\t\t\t{block.Item1} => new {block.Item2}(),");
				//}

				writer.Flush();
			}

			LogGamerules(message.levelSettings.gamerules);

			Client.LevelInfo.LevelName = "Default";
			Client.LevelInfo.Version = 19133;
			Client.LevelInfo.GameType = message.levelSettings.gamemode;

			//ClientUtils.SaveLevel(_level);

			{
				var packet = McpeRequestChunkRadius.CreateObject();
				Client.ChunkRadius = 5;
				packet.chunkRadius = Client.ChunkRadius;

				Client.SendPacket(packet);
			}
		}

		public static string CodeName(string name, bool firstUpper = false)
		{
			//name = name.ToLowerInvariant();

			bool upperCase = firstUpper;

			var result = string.Empty;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == ' ' || name[i] == '_')
				{
					upperCase = true;
				}
				else
				{
					if ((i == 0 && firstUpper) || upperCase)
					{
						result += name[i].ToString().ToUpperInvariant();
						upperCase = false;
					}
					else
					{
						result += name[i];
					}
				}
			}

			result = result.Replace(@"[]", "s");
			return result;
		}

		public override void HandleMcpeAddPlayer(McpeAddPlayer message)
		{
			if (Client.IsEmulator) return;

			Log.DebugFormat("McpeAddPlayer Entity ID: {0}", message.entityIdSelf);
			Log.DebugFormat("McpeAddPlayer Runtime Entity ID: {0}", message.runtimeEntityId);
			Log.DebugFormat("X: {0}", message.x);
			Log.DebugFormat("Y: {0}", message.y);
			Log.DebugFormat("Z: {0}", message.z);
			Log.DebugFormat("Yaw: {0}", message.yaw);
			Log.DebugFormat("Pitch: {0}", message.pitch);
			Log.DebugFormat("Velocity X: {0}", message.speedX);
			Log.DebugFormat("Velocity Y: {0}", message.speedY);
			Log.DebugFormat("Velocity Z: {0}", message.speedZ);
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
				entity.KnownPosition = new PlayerLocation(message.x, message.y, message.z, message.yaw, message.yaw, message.pitch);
				entity.Velocity = new Vector3(message.speedX, message.speedY, message.speedZ);
				Client.Entities.TryAdd(entity.EntityId, entity);
			}

			Log.DebugFormat("McpeAddEntity Entity ID: {0}", message.entityIdSelf);
			Log.DebugFormat("McpeAddEntity Runtime Entity ID: {0}", message.runtimeEntityId);
			Log.DebugFormat("Entity Type: {0}", message.entityType);
			Log.DebugFormat("X: {0}", message.x);
			Log.DebugFormat("Y: {0}", message.y);
			Log.DebugFormat("Z: {0}", message.z);
			Log.DebugFormat("Yaw: {0}", message.yaw);
			Log.DebugFormat("Pitch: {0}", message.pitch);
			Log.DebugFormat("Velocity X: {0}", message.speedX);
			Log.DebugFormat("Velocity Y: {0}", message.speedY);
			Log.DebugFormat("Velocity Z: {0}", message.speedZ);
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
				Vector3 pos = new Vector3(message.x, message.y, message.z);
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
						transaction.transaction = new ItemUseOnEntityTransaction()
						{
							TransactionRecords = new List<TransactionRecord>(),
							EntityId = id,
							ActionType = 0,
							Slot = 0,
							Item = new ItemAir(),
							FromPosition = Client.CurrentLocation,
							ClickPosition = pos,
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

			foreach (Recipe recipe in message.recipes)
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
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (var itemStack in shapelessRecipe.Input)
					{
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
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
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new Item[]");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Input)
					{
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					}
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapedRecipe.Block}\"){{ UniqueId = {shapedRecipe.UniqueId} }},");

					writer.Indent--;

					continue;
				}

				var smeltingRecipe = recipe as SmeltingRecipe;
				if (smeltingRecipe != null)
				{
					writer.WriteLine($"new SmeltingRecipe(new Item({smeltingRecipe.Result.Id}, {smeltingRecipe.Result.Metadata}, {smeltingRecipe.Result.Count}){{ UniqueId = {smeltingRecipe.Result.UniqueId}, RuntimeId={smeltingRecipe.Result.RuntimeId} }}, new Item({smeltingRecipe.Input.Id}, {smeltingRecipe.Input.Metadata}){{ UniqueId = {smeltingRecipe.Input.UniqueId}, RuntimeId={smeltingRecipe.Input.RuntimeId} }}, \"{smeltingRecipe.Block}\"),");
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
			// TODO doesn't work anymore I guess
			if (Client.IsEmulator) return;

			if (message.blobHashes != null)
			{
				var hits = new ulong[message.blobHashes.Length];

				for (int i = 0; i < message.blobHashes.Length; i++)
				{
					ulong hash = message.blobHashes[i];
					hits[i] = hash;
					Log.Debug($"Got hashes for {message.chunkX}, {message.chunkZ}, {hash}");
				}

				var status = McpeClientCacheBlobStatus.CreateObject();
				status.hashHits = hits;
				Client.SendPacket(status);
			}
			else
			{
				Client.Chunks.GetOrAdd(new ChunkCoordinates(message.chunkX, message.chunkZ), coordinates =>
				{
					Log.Debug($"Chunk X={message.chunkX}, Z={message.chunkZ}, size={message.chunkData.Length}, Count={Client.Chunks.Count}");

					ChunkColumn chunk = null;
					try
					{
						chunk = ClientUtils.DecodeChunkColumn((int) message.subChunkCount, message.chunkData);
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

			var root = message.namedtag.NbtFile.RootTag;
			//Log.Debug($"\n{root}");
			File.WriteAllText(Path.Combine(Path.GetTempPath(), "Biomes_" + Guid.NewGuid() + ".txt"), root.ToString());
		}

		public override void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
		{
		}

		public override void HandleMcpePlayStatus(McpePlayStatus message)
		{

			base.HandleMcpePlayStatus(message);

			if (Client.PlayerStatus == 0)
			{
				var packet = McpeClientCacheStatus.CreateObject();
				packet.enabled = Client.UseBlobCache;
				Client.SendPacket(packet);
			}
		}

		/// <inheritdoc />
		public override void HandleMcpeCommandOutput(McpeCommandOutput message)
		{
			base.HandleMcpeCommandOutput(message);

			//foreach (var msg in message.Messages)
			//{
			//	Log.Warn($"Received command output: {msg}");
			//}
		}
	}
}