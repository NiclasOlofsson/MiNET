
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
using MiNET.Worlds;
using Newtonsoft.Json.Linq;

namespace MiNET.Client
{
	public class BedrockTraceHandler : McpeClientMessageHandlerBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockTraceHandler));


		public BedrockTraceHandler(MiNetClient client) : base(client)
		{
		}

		//public override void HandleMcpePlayStatus(McpePlayStatus message)
		//{
		//	throw new NotImplementedException();
		//}

		//public override void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message)
		//{
		//	throw new NotImplementedException();
		//}

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

			sb.AppendLine("Resource packs:");
			foreach (ResourcePackInfo info in message.resourcepackinfos)
			{
				sb.AppendLine($"ID={info.PackIdVersion.Id}, Version={info.PackIdVersion.Version}, Unknown={info.Size}");
			}

			sb.AppendLine("Behavior packs:");
			foreach (ResourcePackInfo info in message.behahaviorpackinfos)
			{
				sb.AppendLine($"ID={info.PackIdVersion.Id}, Version={info.PackIdVersion.Version}");
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
				sb.AppendLine($"ID={info.Id}, Version={info.Version}, Unknown={info.Unknown}");
			}

			sb.AppendLine("Behavior pack stacks:");
			foreach (var info in message.behaviorpackidversions)
			{
				sb.AppendLine($"ID={info.Id}, Version={info.Version}, Unknown={info.Unknown}");
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

			var wantExec = _executioners.Where(e => e.CanExecute(message.message));
			string text = message.message;
			foreach (var executioner in wantExec)
			{
				Log.Debug($"Executing command handler: {executioner.GetType().FullName}");
				Task.Run(() => executioner.Execute(this, text));
			}

			if (message.message.Equals(".do"))
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

			if (message.inventoryId == 0x79)
			{
				string fileName = Path.GetTempPath() + "Inventory_0x79_" + Guid.NewGuid() + ".txt";
				Client.WriteInventoryToFile(fileName, slots);
			}
			else if (message.inventoryId == 0x00)
			{
				//string fileName = Path.GetTempPath() + "Inventory_0x00_" + Guid.NewGuid() + ".txt";
				//Client.WriteInventoryToFile(fileName, slots);
			}
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
			Client.BlockPalette = blockPalette;

			Log.Warn($"Got position from startgame packet: {Client.CurrentLocation}");

			string fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using (FileStream file = File.OpenWrite(fileName))
			{
				Log.Warn($"Writing new blocks to filename:\n{fileName}");

				var legacyIdMap = new Dictionary<string, int>();
				var assembly = Assembly.GetAssembly(typeof(Block));
				using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".legacy_id_map.json"))
				using (var reader = new StreamReader(stream))
				{
					var result = JObject.Parse(reader.ReadToEnd());

					foreach (var obj in result)
					{
						legacyIdMap.Add(obj.Key, (int) obj.Value);
					}
				}

				var writer = new IndentedTextWriter(new StreamWriter(file));

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;

				var blocks = new List<(int, string)>();

				foreach (IGrouping<string, BlockStateContainer> blockstate in blockPalette.OrderBy(record => record.Name).ThenBy(record => record.Data).GroupBy(record => record.Name))
				{
					var enumerator = blockstate.GetEnumerator();
					enumerator.MoveNext();
					var value = enumerator.Current;
					if (value == null) continue;
					Log.Debug($"{value.RuntimeId}, {value.Name}, {value.Data}");
					//int id = BlockFactory.GetBlockIdByName(value.Name.Replace("minecraft:", ""));
					Block blockById = BlockFactory.GetBlockById(value.Id);
					bool existingBlock = blockById.GetType() != typeof(Block) && !blockById.IsGenerated;
					int id = existingBlock ? value.Id : 0;

					if (!(blockById is Air))
					{
						//if (legacyIdMap.TryGetValue(value.Name, out id))
						//{
						//	value.Id = id;
						//}

						string blockName = Client.CodeName(value.Name.Replace("minecraft:", ""), true);

						blocks.Add((value.Id, blockName));

						writer.WriteLineNoTabs($"");

						writer.WriteLine($"public partial class {blockName} {(existingBlock ? "" : ": Block")} // {blockById.Id} typeof={blockById.GetType().Name}");
						writer.WriteLine($"{{");
						writer.Indent++;

						var bits = new List<BlockStateByte>();
						foreach (var state in blockstate.First().States)
						{
							var q = blockstate.SelectMany(c => c.States);

							// If this is on base, skip this property. We need this to implement common functionality.
							Type baseType = blockById.GetType().BaseType;
							bool propOverride = baseType != null
												&& ("Block" != baseType.Name
													&& baseType.GetProperty(Client.CodeName(state.Name, true)) != null);

							switch (state)
							{
								case BlockStateByte blockStateByte:
								{
									var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateByte) d).Value).Distinct().OrderBy(s => s).ToList();
									if (values.Min() == 0 && values.Max() == 1)
									{
										bits.Add(blockStateByte);
										writer.Write($"[StateBit] ");
										writer.WriteLine($"public {(propOverride ? "override" : "")} bool {Client.CodeName(state.Name, true)} {{ get; set; }}");
									}
									else
									{
										writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
										writer.WriteLine($"public {(propOverride ? "override" : "")} byte {Client.CodeName(state.Name, true)} {{ get; set; }}");
									}
									break;
								}
								case BlockStateInt blockStateInt:
								{
									var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateInt) d).Value).Distinct().OrderBy(s => s).ToList();
									writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
									writer.WriteLine($"public {(propOverride ? "override" : "")} int {Client.CodeName(state.Name, true)} {{ get; set; }}");
									break;
								}
								case BlockStateString blockStateString:
								{
									var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateString) d).Value).Distinct().OrderBy(s => s).ToList();
									if (values.Count > 1)
									{
										writer.WriteLine($"[StateEnum({string.Join(',', values.Select(v => $"\"{v}\""))})]");
									}
									writer.WriteLine($"public {(propOverride ? "override" : "")} string {Client.CodeName(state.Name, true)} {{ get; set; }}");
									break;
								}
								default:
									throw new ArgumentOutOfRangeException(nameof(state));
							}
						}

						if (id == 0 || blockById.IsGenerated)
						{
							writer.WriteLine($"");

							writer.WriteLine($"public {blockName}() : base({value.Id})");
							writer.WriteLine($"{{");
							writer.Indent++;
							writer.WriteLine($"IsGenerated = true;");
							writer.Indent--;
							writer.WriteLine($"}}");
						}

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

						foreach (var state in blockstate.First().States)
						{
							writer.WriteLine($"case {state.GetType().Name} s when s.Name == \"{state.Name}\":");
							writer.Indent++;
							writer.WriteLine($"{Client.CodeName(state.Name, true)} = {(bits.Contains(state) ? "Convert.ToBoolean(s.Value)" : "s.Value")};");
							writer.WriteLine($"break;");
							writer.Indent--;
						}

						writer.Indent--;
						writer.WriteLine($"}} // switch");
						writer.Indent--;
						writer.WriteLine($"}} // foreach");
						writer.Indent--;
						writer.WriteLine($"}} // method");

						//public BlockStateContainer GetState()
						//{
						//	var record = new BlockStateContainer();
						//	record.Name = "";
						//	record.Id = Id;
						//	record.States.Add(new BlockStateByte() {Name = "", Value = ButtonPressedBit});
						//	record.States.Add(new BlockStateInt() {Name = "", Value = FacingDirection});

						//	return record;
						//}

						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override BlockStateContainer GetState()");
						writer.WriteLine($"{{");
						writer.Indent++;
						writer.WriteLine($"var record = new BlockStateContainer();");
						writer.WriteLine($"record.Name = \"{blockstate.First().Name}\";");
						writer.WriteLine($"record.Id = {blockstate.First().Id};");
						foreach (var state in blockstate.First().States)
						{
							string propName = Client.CodeName(state.Name, true);
							writer.WriteLine($"record.States.Add(new {state.GetType().Name} {{Name = \"{state.Name}\", Value = {(bits.Contains(state) ? $"Convert.ToByte({propName})" : propName)}}});");
						}
						writer.WriteLine($"return record;");
						writer.Indent--;
						writer.WriteLine($"}} // method");

						//writer.WriteLine($"");

						//writer.WriteLine($"public byte GetMetadataFromState()");
						//writer.WriteLine($"{{");
						//writer.Indent++;

						//writer.WriteLine($"switch(this)");
						//writer.WriteLine($"{{");
						//writer.Indent++;


						//i = 0;
						//foreach (var record in message.BlockPalette.Where(b => b.Id == enumerator.Current.Id).OrderBy(b => b.Data))
						//{
						//	//case { } b when b.ButtonPressedBit == 0 && b.FacingDirection == 0:
						//	//	return 0;

						//	writer.Write($"case {{ }} b when true");
						//	string retVal = "";
						//	foreach (var state in record.States.OrderBy(s => s.Name).ThenBy(s => s.Value))
						//	{
						//		if (state.Type == (byte) NbtTagType.Byte)
						//		{
						//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == {state.Value}");
						//		}
						//		else if (state.Type == (byte) NbtTagType.Int)
						//		{
						//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == {state.Value}");
						//		}
						//		else if (state.Type == (byte) NbtTagType.String)
						//		{
						//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == \"{state.Value}\"");
						//		}
						//	}
						//	writer.WriteLine($":");

						//	writer.Indent++;
						//	writer.WriteLine($"return { i++ };");
						//	writer.Indent--;
						//}

						//writer.Indent--;
						//writer.WriteLine($"}} // switch");

						//writer.WriteLine($"throw new ArithmeticException(\"Invalid state. Unable to convert state to valid metadata\");");

						//writer.Indent--;
						//writer.WriteLine($"}} // method");

						writer.Indent--;
						writer.WriteLine($"}} // class");
					}
				}

				writer.Indent--;
				writer.WriteLine($"}}");

				foreach (var block in blocks.OrderBy(tuple => tuple.Item1))
				{
					writer.WriteLine($"else if (blockId == {block.Item1}) block = new {block.Item2}();");
				}

				writer.Flush();
			}

			//			Log.Debug($@"
			//StartGame:
			//	entityId: {message.entityIdSelf}	
			//	runtimeEntityId: {message.runtimeEntityId}	
			//	spawn: {message.spawn}	
			//	unknown1: {message.unknown1}	
			//	dimension: {message.dimension}	
			//	generator: {message.generator}	
			//	gamemode: {message.gamemode}	
			//	difficulty: {message.difficulty}	
			//	hasAchievementsDisabled: {message.hasAchievementsDisabled}	
			//	dayCycleStopTime: {message.dayCycleStopTime}	
			//	eduMode: {message.eduMode}	
			//	rainLevel: {message.rainLevel}	
			//	lightnigLevel: {message.lightnigLevel}	
			//	enableCommands: {message.enableCommands}	
			//	isTexturepacksRequired: {message.isTexturepacksRequired}	
			//	secret: {message.levelId}	
			//	worldName: {message.worldName}	
			//");

			LogGamerules(message.gamerules);


			Client.LevelInfo.LevelName = "Default";
			Client.LevelInfo.Version = 19133;
			Client.LevelInfo.GameType = message.gamemode;

			//ClientUtils.SaveLevel(_level);

			{
				var packet = McpeRequestChunkRadius.CreateObject();
				Client.ChunkRadius = 5;
				packet.chunkRadius = Client.ChunkRadius;

				Client.SendPacket(packet);
			}
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
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}),");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (var itemStack in shapelessRecipe.Input)
					{
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}),");
					}
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapelessRecipe.Block}\"),");

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
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}),");
					}
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new Item[]");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Input)
					{
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}),");
					}
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapedRecipe.Block}\"),");

					writer.Indent--;

					continue;
				}

				var smeltingRecipe = recipe as SmeltingRecipe;
				if (smeltingRecipe != null)
				{
					writer.WriteLine($"new SmeltingRecipe(new Item({smeltingRecipe.Result.Id}, {smeltingRecipe.Result.Metadata}, {smeltingRecipe.Result.Count}), new Item({smeltingRecipe.Input.Id}, {smeltingRecipe.Input.Metadata}), \"{smeltingRecipe.Block}\"),");
					continue;
				}

				var multiRecipe = recipe as MultiRecipe;
				if (multiRecipe != null)
				{
					writer.WriteLine($"new MultiRecipe() {{ Id = new UUID(\"{recipe.Id}\") }}, // {recipe.Id}");
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
	}
}