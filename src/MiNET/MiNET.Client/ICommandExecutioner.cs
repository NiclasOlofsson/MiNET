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
using System.Reflection;
using System.Threading;
using log4net;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiNET.Client
{
	public interface ICommandExecutioner
	{
		public bool CanExecute(string text);
		public void Execute(BedrockTraceHandler caller, string text);
	}

	public interface IGenericPacketHandler
	{
		public void HandlePacket(BedrockTraceHandler caller, Packet packet);
	}

	public class PlaceAllBlocksExecutioner : ICommandExecutioner, IGenericPacketHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PlaceAllBlocksExecutioner));

		public bool CanExecute(string text)
		{
			return text.Contains("all blocks")
					|| text.Contains("discover drops")
					|| text.Contains("write palette")
					|| text.Contains("pick blocks")
					|| text.Contains("print palette")
					|| text.Contains("all items");
		}

		private bool _runningBlockMetadataDiscovery;

		public void Execute(BedrockTraceHandler caller, string text)
		{
			if (text.Contains("all blocks"))
			{
				ExecuteAllBlocks(caller);
			}
			else if (text.Contains("discover drops"))
			{
				ExecuteDiscoveryOfItemDrops(caller);
			}
			else if (text.Contains("write palette"))
			{
				ExecuteWritePallet(caller);
			}
			else if (text.Contains("pick blocks"))
			{
				ExecutePickBlocks(caller);
			}
			else if (text.Contains("print palette"))
			{
				PrintPalette(caller);
			}
			else if (text.Contains("all items"))
			{
				DiscoverItems(caller);
			}
			else
			{
				Log.Debug($"Found no matching method for '{text}'");
			}
		}

		private void ClearInventory(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			SendCommand(client, $"/clear TheGrey");
		}
		
		private AutoResetEvent _resetEventInventorySlot = new AutoResetEvent(false);
		private Item _lastItem = new ItemAir()
		{
			Count = 0
		};

		private void HandleInventorySlot(McpeInventorySlot packet)
		{
			lock (_lastItem)
			{
				var item = packet.item;

				if (item != null && item is not ItemAir && item.Count > 0)
				{
					_lastItem = item;
					_resetEventInventorySlot.Set();
				}
			}
		}

		private void DiscoverItems(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			client.SendChat("Starting item discovery...");
			
			ClearInventory(caller);

			Dictionary<string, short> idMapping = new Dictionary<string, short>();
			
			//for(int i = -400; i < 900; i++)
			{
			//	for (short meta = 0; meta < 15; meta++)
				{
					foreach (var itemType in typeof(Item).Assembly.GetTypes().Where(x => typeof(Item).IsAssignableFrom(x)))
					{
						try
						{
							var item = Activator.CreateInstance(itemType) as Item;

							if (item == null || string.IsNullOrWhiteSpace(item?.Name) || item is ItemAir)
							{
								continue;
							}
							
							string itemName = item.Name;

							if (ItemFactory.Translator.TryGetName(itemName, out var newName))
							{
								Log.Warn($"Name mistmatch for item: {item} (Current={item.Name} New={newName})");
								itemName = newName;
							}

							if (idMapping.ContainsKey(item.Name))
								continue;

							ClearInventory(caller);
							SendCommand(client, $"/give TheGrey {itemName}");

							if (!_resetEventInventorySlot.WaitOne(500))
							{
								Log.Warn($"Failed to get item: {itemName}");

								continue;
							}

							lock (_lastItem)
							{
								var newItem = _lastItem;

								if (newItem != null && (newItem is not ItemAir && newItem.Count > 0))
								{
									if (!idMapping.TryAdd(item.Name, newItem.Id))
									{
										Log.Warn($"Duplicate key! Name={item.Name} Id={item.Id} NewName={newItem.Name} NewId={newItem.Id}");
									}
								}

								_lastItem = new ItemAir() { Count = 0 };
							}
						}catch{}
					}
				}
			}
			
			client.SendChat($"Finished item discovery...");
			var fileNameItemstates = Path.GetTempPath() + "itemdiscovery_" + Guid.NewGuid() + ".json";
			/*File.WriteAllText(fileNameItemstates, JsonConvert.SerializeObject(items.Distinct(new ItemEqualityComparer()).Select(x => new
			{
				Name = x.Name,
				Id = x.Id
			}).ToArray(), Formatting.Indented));*/
			
			File.WriteAllText(fileNameItemstates, JsonConvert.SerializeObject(idMapping, Formatting.Indented));
		}

		private void PrintPalette(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			var palette = client.BlockPalette;
			foreach (BlockStateContainer blockState in palette.OrderByDescending(bs => bs.Id))
			{
				Log.Warn($"{blockState.Name}");
			}
		}

		private void ExecutePickBlocks(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			int count = 600;
			int yStart = 100;
			int x = 0;
			int z = 0;

			SendCommand(client, $"/clear TheGrey");

			string fileName = Path.GetTempPath() + "pick_items_" + Guid.NewGuid() + ".json";
			var writer = File.AppendText(fileName);

			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.None,
			};
			jsonSerializerSettings.Converters.Add(new NbtIntConverter());
			jsonSerializerSettings.Converters.Add(new NbtStringConverter());

			//BlockPalette palette = client.BlockPalette;
			BlockPalette palette = BlockFactory.BlockPalette; // only if we have updated it

			_resetEventPlayerHotbar.Reset();

			for (int id = 0; id < count; id++)
			{
				try
				{
					SendCommand(client, $"/tp TheGrey {x} {150} {z}");

					int y = yStart;
					for (int meta = 0; meta <= 15; meta++)
					{
						var blockstate = palette.FirstOrDefault(b => b.Id == id && b.Data == meta);
						if (blockstate == null) continue;
						blockstate.ItemInstance = null; // reset to nothing

						string name = blockstate.Name.Replace("minecraft:", "");
						if (name == "double_plant" || name == "air" /*|| name.StartsWith("element")*/) break;

						var pick = McpeBlockPickRequest.CreateObject();
						pick.x = x;
						pick.y = y;
						pick.z = z;
						client.SendPacket(pick);

						//Thread.Sleep(100);
						if (!_resetEventPlayerHotbar.WaitOne(300))
						{
							Log.Error($"No picked item for {id}, {meta}, {name}");
							_lastSelectedItem = new ItemAir();
							continue;
							//break;
						}

						// Investigate last selected item. This should be the one we picked
						Item item;
						lock (_lastSelectedItem)
						{
							item = _lastSelectedItem;
							_lastSelectedItem = new ItemAir();
						}
						Log.Warn($"For {id}, {meta} we picked {item}");
						blockstate.ItemInstance = new ItemPickInstance()
						{
							Id = item.Id,
							Metadata = item.Metadata,
							WantNbt = item.ExtraData != null
						};
						string result = JsonConvert.SerializeObject(blockstate, jsonSerializerSettings);
						writer.WriteLine($"{item}; {result}");
						writer.Flush();

						if (blockstate.States.Count == 0)
							break;

						y += 2;
					}
				}
				finally
				{
					x += 2;
				}
			}

			writer.Close();

			WritePaletteToJson(palette);
			Log.Warn("Finished!");
		}

		private void ExecuteWritePallet(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			BlockPalette palette = client.BlockPalette;

			_runningBlockMetadataDiscovery = false;

			WritePaletteToJson(palette);
		}

		private static void WritePaletteToJson(BlockPalette palette)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.None,
			};
			jsonSerializerSettings.Converters.Add(new StringEnumConverter());
			jsonSerializerSettings.Converters.Add(new NbtIntConverter());
			jsonSerializerSettings.Converters.Add(new NbtStringConverter());
			jsonSerializerSettings.Converters.Add(new IPAddressConverter());
			jsonSerializerSettings.Converters.Add(new IPEndPointConverter());


			var records = palette.OrderBy(bs => bs.Id).ThenBy(bs => bs.Data).ToArray();

			string fileName = Path.GetTempPath() + "blockstates_" + Guid.NewGuid() + ".json";
			var writer = File.AppendText(fileName);
			bool isFirst = true;
			writer.WriteLine("[");
			foreach (var record in records)
			{
				string result = JsonConvert.SerializeObject(record, jsonSerializerSettings);

				writer.Write($"{(isFirst ? "" : ",\n")}{result}");
				isFirst = false;
			}
			writer.WriteLine("\n]");

			var stringRecords = records.Where(r => r.States.Any(s => s is BlockStateString));
			foreach (BlockStateContainer stringRecord in stringRecords)
			{
				writer.WriteLine($"Item {stringRecord.Name}");
			}

			writer.Close();
		}

		private void ExecuteDiscoveryOfItemDrops(BedrockTraceHandler caller)
		{
			var client = caller.Client;

			{
				SendCommand(client, $"/gamerule dotiledrops true");
			}

			int x = 0;
			int yStart = 100;
			int z = 0;

			{
				SendCommand(client, $"/tp TheGrey {x} {150} {z}");
			}

			{
				for (int xd = x - 1; xd <= x + 1; xd++)
				{
					for (int zd = z - 1; zd <= z + 1; zd++)
					{
						for (int yd = yStart - 1; yd < yStart + 34; yd++)
						{
							{
								SendCommand(client, $"/setblock {xd} {yd} {zd} glass 0 replace");
							}

							{
								SendCommand(client, $"/setblock {xd} {yd} {zd} barrier 0 replace");
							}
						}
					}
				}
			}

			x = 0;
			z = 0;

			Log.Warn("Running!");
			_resetEventAddItemEntity.Reset();

			int count = 600;
			int y = yStart;

			for (int id = 1; id < count; id++)
			{
				try
				{
					var blockstate = client.BlockPalette.FirstOrDefault(b => b.Id == id);
					string name = blockstate.Name.Replace("minecraft:", "");

					//if (name.StartsWith("element")) continue;

					client.BlockPalette.Where(bs => bs.Id == id).ToList().ForEach(
						bs =>
						{
							bs.Data = -1;
						});
					
					SendCommand(client, $"/setblock {x} {y} {z} air 0 replace");
					Log.Warn($"Setting block {id} {name}");

					for (int meta = 0; meta <= 15; meta++)
					{
						try
						{
							Log.Debug($"Setting block {id} {meta} {name}");
							SendCommand(client, $"/setblock {x} {y} {z} {name} {meta} replace");
							SendCommand(client, $"/setblock {x} {y} {z} air 0 destroy");
							
							if (!_resetEventAddItemEntity.WaitOne(1000))
								break;
						}
						finally
						{
							SendCommand(client, $"/kill @e[r=150]");
						}

						if (blockstate.States.Count == 0)
							break;
					}
				}
				finally { }
			}

			client.SendChat($"Finished setting blocks.");
			Log.Warn("Finished!");
		}

		private static void SendCommand(MiNetClient client, string command)
		{
			var request = McpeCommandRequest.CreateObject();
			request.command = command;
			request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			client.SendPacket(request);
		}

		private void SetGameRules(BedrockTraceHandler caller)
		{
			var client = caller.Client;
			SendCommand(client, $"/gamerule randomtickspeed 0");
			SendCommand(client, $"/gamerule doentitydrops false");
			SendCommand(client, $"/gamerule domobloot false");
			SendCommand(client, $"/gamerule domobspawning false");
			SendCommand(client, $"/gamerule dotiledrops false");
			SendCommand(client, $"/gamerule mobgriefing false");
			SendCommand(client, $"/gamerule doweathercycle false");
			SendCommand(client, $"/gamerule showcoordinates true");
			
			SendCommand(client, $"/gamerule dotiledrops false");
		}
		
		private void ExecuteAllBlocks(BedrockTraceHandler caller)
		{
			var client = caller.Client;

			if (_runningBlockMetadataDiscovery) return;

			client.SendChat("Starting...");

			SetGameRules(caller);
			int count = 600;

			void Report(string message)
			{
				Log.Warn(message);
				client.SendChat(message);
			}

			var width = (int)MathF.Ceiling(MathF.Sqrt(count));
			var depth = (int)MathF.Ceiling((float)count / width);
			
			
			const int yStart = 100;
			_blockCoordinates.Clear();
			var prevProgress = -1d;
			for (int bx = -width; bx < width; bx += 2)
			{
				if (_blockCoordinates.Count >= count)
					break;
				
				for (int bz = -depth; bz < depth; bz += 2)
				{
					if (_blockCoordinates.Count >= count)
						break;
					
					if (bx % 10 == 0)
					{
						SendCommand(client, $"/tp TheGrey {bx} {(yStart + 50)} {bz}");
					}

					_blockCoordinates.Add(new BlockCoordinates(bx, 0, bz));
					
					for (int yd = yStart; yd < yStart + 33; yd++)
					{

						SendCommand(client, $"/setblock {bx} {yd} {bz} log 0 replace");
							//if (!_resetEventUpdateBlock.WaitOne(250)) Log.Warn("wait timeout");

						SendCommand(client, $"/setblock {bx} {yd} {bz} barrier 0 replace");
							//if (!_resetEventUpdateBlock.WaitOne(250)) Log.Warn("wait timeout");

						for (int xd = bx - 1; xd <= bx + 1; xd++)
						{
							for (int zd = bz - 1; zd <= bz + 1; zd++)
							{
								SendCommand(client, $"/setblock {xd} {yd} {zd} air 0 replace");
								SendCommand(client, $"/setblock {xd} {yd} {zd} barrier 0 replace");
							}
						}
					}

					//if (bx % 2 == 0 && bz % 2 == 0)
					{
					
						var progress = ((double)_blockCoordinates.Count / (count)) * 100d;

						if ((int)progress > prevProgress)
						{
							Report($"Prep Progress: {(int)progress}%");
							prevProgress = progress;
						}
					}
				}
			}

			prevProgress = -1;
			_resetEventUpdateBlock.Reset();
			_resetEventPlayerHotbar.Reset();
			
			var cQueue = new Queue<BlockCoordinates>(_blockCoordinates);
			SendCommand(client, $"/tp TheGrey 0 150 0");

			Report("Staring to run in 1s");
			Thread.Sleep(1000);
			Report("Running!");
			
		//	client.BlockPalette = BlockFactory.BlockPalette;
			client.BlockPalette.ForEach(bs => { bs.Data = -1; });
			_runningBlockMetadataDiscovery = true;
			_resetEventUpdateBlock.Reset();
			_resetEventPlayerHotbar.Reset();

			for (int id = 0; id < count; id++)
			{
				var progress = ((double)id / (count)) * 100d;

				if ((int)progress > prevProgress)
				{
					Report($"Place Progress: {(int)progress}%");
					prevProgress = progress;
				}

				var c = cQueue.Dequeue();
				try
				{
					SendCommand(client, $"/tp TheGrey {c.X} {150} {c.Z}");

					//var block = BlockFactory.GetBlockById(id);
					//if (block.GetType() == typeof(Block))
					//{
					//	continue;
					//}

					var blockstate = client.BlockPalette.FirstOrDefault(b => b.Id == id);
					if (blockstate == null) continue;

					string name = blockstate.Name.Replace("minecraft:", "");

					if ("air".Equals(name))
					{
						blockstate.Data = 0;
						continue; // don't want
					}

					//if (name.StartsWith("element")) continue;

					int y = yStart;
					for (int meta = 0; meta <= 15; meta++)
					{
					//	Log.Warn($"Setting block {id} {meta} {name}");

						SendCommand(client, $"/setblock {c.X} {y} {c.Z} {name} {meta} replace");

						if (!_resetEventUpdateBlock.WaitOne(500))
						{
							SendCommand(client, $"/setblock {c.X} {y} {c.Z} air 0 replace");
							//break;
						}

						lock (_lastUpdatedBlockstate)
						{
							if (_lastUpdatedBlockstate.Id != blockstate.Id) Log.Warn($"Got wrong ID for blockstate. Expected {blockstate.Id}, got {_lastUpdatedBlockstate.Id} (Expected: {blockstate.Name} Got: {_lastUpdatedBlockstate.Name})");

							var minetBlockstate = GetServerRuntimeId(client.BlockPalette, _internalStates, _lastUpdatedBlockstate.RuntimeId);
							if (minetBlockstate != null)
							{
								if (minetBlockstate.Id != _lastUpdatedBlockstate.Id) Log.Warn($"Blockstate BID is different between MiNET and bedrock?");
								if (minetBlockstate.Data != _lastUpdatedBlockstate.Data) Log.Warn($"Blockstate data is different between MiNET and bedrock? {minetBlockstate}, {_lastUpdatedBlockstate}");
							}
						}

						if (blockstate.States.Count == 0) break;

						y += 2;
					}
				}
				finally
				{
					//x += 2;
				}
			}

			_runningBlockMetadataDiscovery = false;

			WritePaletteToJson(client.BlockPalette);

			Report($"Finished setting blocks.");
		}

		HashSet<BlockStateContainer> _internalStates = new HashSet<BlockStateContainer>(BlockFactory.BlockPalette);

		private static BlockStateContainer GetServerRuntimeId(BlockPalette bedrockPalette, HashSet<BlockStateContainer> internalBlockPallet, int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= bedrockPalette.Count)
				Log.Error($"RuntimeId = {runtimeId}");

			var record = bedrockPalette[runtimeId];

			if (!internalBlockPallet.TryGetValue(record, out BlockStateContainer internalRecord))
			{
				Log.Error($"Did not find {record.Id}");
				return null;
			}

			return internalRecord;
		}

		public void HandlePacket(BedrockTraceHandler caller, Packet packet)
		{
			switch (packet)
			{
				case McpeAddItemEntity mcpePacket:
					HandleMcpeAddItemEntity(mcpePacket);
					break;
				case McpeUpdateBlock mcpePacket:
					HandleMcpeUpdateBlock(caller, mcpePacket);
					break;
				case McpePlayerHotbar mcpePacket:
					HandleMcpePlayerHotbar(mcpePacket);
					break;
				case McpeInventoryContent mcpePacket:
					HandleMcpeInventoryContent(mcpePacket);
					break;
				case McpeInventorySlot mcpePacket:
					HandleInventorySlot(mcpePacket);
					break;
				default:
					return;
			}
		}

		private AutoResetEvent _resetEventPlayerHotbar = new AutoResetEvent(false);

		private Item _lastSelectedItem = new ItemAir();

		private void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
			lock (_lastSelectedItem)
			{
				_lastSelectedItem = _inventory[(int) message.selectedSlot];
				_resetEventPlayerHotbar.Set();
			}
		}

		private ItemStacks _inventory;

		private void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
			if (message.inventoryId == 0x00)
			{
				_inventory = message.input;

				if (message.input.Count > 0)
				{
					lock (_lastItem)
					{
						var item = message.input.FirstOrDefault(x => x.Count > 0);

						if (item != null && item is not ItemAir && item.Count > 0)
						{
							_lastItem = item;
							_resetEventInventorySlot.Set();
						}
					}
				}
			}
		}

		private AutoResetEvent _resetEventAddItemEntity = new AutoResetEvent(false);

		private void HandleMcpeAddItemEntity(McpeAddItemEntity message)
		{
			Log.Warn($"Dropped item {message.item}");
			_resetEventAddItemEntity.Set();
		}

		private AutoResetEvent _resetEventUpdateBlock = new AutoResetEvent(false);

		private BlockStateContainer _lastUpdatedBlockstate = new BlockStateContainer();

		private int lastNumber = 0;
		private List<BlockCoordinates> _blockCoordinates = new List<BlockCoordinates>();

		private void HandleMcpeUpdateBlock(BedrockTraceHandler caller, McpeUpdateBlock message)
		{
			//if (message.OrderingIndex <= lastNumber) return;
			//lastNumber = message.OrderingIndex;

			if (message.storage != 0) return;
			if (message.blockPriority != 3) return;

			if (!_runningBlockMetadataDiscovery)
			{
				_resetEventUpdateBlock.Set();
				return;
			}

			int runtimeId = (int) message.blockRuntimeId;
			int bid = message.coordinates.X / 2;
			int meta = (message.coordinates.Y - 100) / 2;

			//TODO: Fix for doors and beds. They get 2 updates.
			BlockStateContainer blockstate = caller.Client.BlockPalette[runtimeId];
			if (!_blockCoordinates.Contains(new BlockCoordinates(message.coordinates.X, 0, message.coordinates.Z)))
			{
				Log.Warn($"Update block outside of grid {message.coordinates}, {caller.Client.BlockPalette[runtimeId]}");

				if (blockstate.Name.EndsWith("_door"))
				{
					if (blockstate.States.First(s => s.Name.Equals("upper_block_bit") && ((BlockStateInt) s).Value == 1) != null)
						blockstate.Data = (short) meta;
				}
				else if (blockstate.Name.Equals("minecraft:bed"))
				{
					if (blockstate.States.First(s => s.Name.Equals("head_piece_bit") && ((BlockStateInt) s).Value == 0) != null)
						blockstate.Data = (short) meta;
				}

				_resetEventUpdateBlock.Set();
				return;
			}

			if (blockstate.Id == 0)
			{
				_resetEventUpdateBlock.Set();
				return;
			}

			if (blockstate.Data == -1)
			{
				lock (_lastUpdatedBlockstate)
				{
					try
					{
						if (bid != blockstate.Id)
						{
							Log.Warn($"Wrong id. Expected {blockstate.Id}, got {bid}");
						}
						else
						{
							blockstate.Data = (short) meta;

							Log.Debug($"Correct id. Expected {blockstate.Id}, and got {bid}");
						}

						Log.Debug($"Block update {bid}, {meta}, with blockstate\n{blockstate}");
					}
					finally
					{
						Log.Warn($"Got {blockstate.Id}, {meta} storage {message.storage}, {message.blockPriority} (Name={blockstate.Name} Id={blockstate.Id})");
						_lastUpdatedBlockstate = blockstate;
						_resetEventUpdateBlock.Set();
					}
				}
			}
			else
			{
				Log.Warn($"Blockstate {runtimeId} {blockstate.Id}, {meta} already had meta set to {blockstate.Data}");
				_resetEventUpdateBlock.Set();
			}
		}
	}
}