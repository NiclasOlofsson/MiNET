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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using fNbt;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public interface ICustomBlockFactory
	{
		Block GetBlockById(int blockId);
	}

	public class R12ToCurrentBlockMapEntry
	{
		public string StringId { get; set; }
		public short Meta { get; set; }
		public BlockStateContainer State { get; set; }

		public R12ToCurrentBlockMapEntry(string id, short meta, BlockStateContainer state)
		{
			StringId = id;
			Meta = meta;
			State = state;
		}
	}
	
	public static class BlockFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockFactory));

		public static ICustomBlockFactory CustomBlockFactory { get; set; }

		public static byte[] TransparentBlocks { get; private set; }
		public static byte[] LuminousBlocks { get; private set; }

		public static Dictionary<string, BlockStateContainer> MetaBlockNameToState { get; private set; } = new Dictionary<string, BlockStateContainer>();
		public static Dictionary<int, string> RuntimeIdToId { get; private set; }
		public static Dictionary<string, Type> IdToType { get; private set; } = new Dictionary<string, Type>();
		public static Dictionary<Type, string> TypeToId { get; private set; } = new Dictionary<Type, string>();
		public static Dictionary<string, Func<Block>> IdToFactory { get; private set; } = new Dictionary<string, Func<Block>>();
		public static Dictionary<string, string> ItemToBlock { get; private set; }

		public static List<string> Ids { get; set; }
		public static BlockPalette BlockPalette { get; } = new BlockPalette();
		public static HashSet<BlockStateContainer> BlockStates { get; set; }

		private static readonly object _lockObj = new object();

		static BlockFactory()
		{
			var assembly = Assembly.GetAssembly(typeof(Block));

			lock (_lockObj)
			{
				int runtimeId = 0;
				var ids = new HashSet<string>();

				using (var stream = assembly.GetManifestResourceStream(typeof(BlockFactory).Namespace + ".Data.canonical_block_states.nbt"))
				{
					do
					{
						var compound = Packet.ReadNbtCompound(stream, true);
						var container = GetBlockStateContainer(compound);

						container.RuntimeId = runtimeId++;
						ids.Add(container.Id);
						BlockPalette.Add(container);
					} while (stream.Position < stream.Length);
				}

				Ids = ids.ToList();

				var visitedContainers = new HashSet<BlockStateContainer>();
				var blockMapEntry = new List<R12ToCurrentBlockMapEntry>();

				using (var stream = assembly.GetManifestResourceStream(typeof(BlockFactory).Namespace + ".Data.r12_to_current_block_map.bin"))
				{
					while (stream.Position < stream.Length)
					{
						var length = VarInt.ReadUInt32(stream);
						byte[] bytes = new byte[length];
						stream.Read(bytes, 0, bytes.Length);

						string stringId = Encoding.UTF8.GetString(bytes);

						bytes = new byte[2];
						stream.Read(bytes, 0, bytes.Length);
						var meta = BitConverter.ToInt16(bytes);

						var compound = Packet.ReadNbtCompound(stream, true);

						var state = GetBlockStateContainer(compound);

						if (!visitedContainers.TryGetValue(state, out _))
						{
							blockMapEntry.Add(new R12ToCurrentBlockMapEntry(stringId, meta, state));
							visitedContainers.Add(state);
						}
					}
				}

				Dictionary<string, List<int>> idToStatesMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

				Dictionary<string, string> blockIdItemIdMap = ResourceUtil.ReadResource<Dictionary<string, string>>("block_id_to_item_id_map.json", typeof(BlockFactory), "Data");
				ItemToBlock = blockIdItemIdMap.ToDictionary(pair => pair.Value, pair => pair.Key);

				for (var index = 0; index < BlockPalette.Count; index++)
				{
					var state = BlockPalette[index];
					if (!idToStatesMap.TryGetValue(state.Id, out var candidates))
					{
						candidates = new List<int>();
					}

					candidates.Add(index);
					idToStatesMap[state.Id] = candidates;
				}

				foreach (var entry in blockMapEntry)
				{
					var data = entry.Meta;

					var mappedState = entry.State;
					var mappedName = entry.State.Id;

					if (!idToStatesMap.TryGetValue(mappedName, out var matching))
					{
						continue;
					}

					foreach (var match in matching)
					{
						var networkState = BlockPalette[match];

						var thisStates = new HashSet<IBlockState>(mappedState.States);
						var otherStates = new HashSet<IBlockState>(networkState.States);

						otherStates.IntersectWith(thisStates);

						if (otherStates.Count == thisStates.Count)
						{
							BlockPalette[match].Data = data;

							var id = blockIdItemIdMap.GetValueOrDefault(mappedName, mappedName);

							BlockPalette[match].ItemInstance = new ItemPickInstance()
							{
								Id = id,
								Metadata = data,
								WantNbt = false
							};

							break;
						}
					}
				}

				foreach (var record in BlockPalette)
				{
					var states = new List<NbtTag>();
					foreach (IBlockState state in record.States)
					{
						NbtTag stateTag = null;
						switch (state)
						{
							case BlockStateByte blockStateByte:
								stateTag = new NbtByte(state.Name, blockStateByte.Value);
								break;
							case BlockStateInt blockStateInt:
								stateTag = new NbtInt(state.Name, blockStateInt.Value);
								break;
							case BlockStateString blockStateString:
								stateTag = new NbtString(state.Name, blockStateString.Value);
								break;
							default:
								throw new ArgumentOutOfRangeException(nameof(state));
						}
						states.Add(stateTag);
					}

					record.StatesNbt = new NbtCompound("states", states);

					var nbt = new NbtFile()
					{
						BigEndian = false,
						UseVarInt = true,
						RootTag = record.StatesNbt
					};

					byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);

					record.StatesCacheNbt = nbtBinary;

					MetaBlockNameToState.TryAdd(GetMetaBlockName(record.Id, record.Data), record);
				}

				BlockStates = new HashSet<BlockStateContainer>(BlockPalette);

				(IdToType, TypeToId) = BuildIdTypeMapPair();
				IdToFactory = BuildIdToFactory();
				RuntimeIdToId = BuildRuntimeIdToId();
				(TransparentBlocks, LuminousBlocks) = BuildTransperentAndLuminousMapPair();
			}
		}

		private static BlockStateContainer GetBlockStateContainer(NbtTag tag)
		{
			var record = new BlockStateContainer();

			string name = tag["name"].StringValue;
			record.Id = name;
			record.States = GetBlockStates(tag);

			return record;
		}

		public static List<IBlockState> GetBlockStates(NbtTag tag)
		{
			var result = new List<IBlockState>();

			var states = tag["states"] ?? tag;
			if (states != null && states is NbtCompound compound)
			{
				foreach (var stateEntry in compound)
				{
					switch (stateEntry)
					{
						case NbtInt nbtInt:
							result.Add(new BlockStateInt()
							{
								Name = nbtInt.Name,
								Value = nbtInt.Value
							});
							break;
						case NbtByte nbtByte:
							result.Add(new BlockStateByte()
							{
								Name = nbtByte.Name,
								Value = nbtByte.Value
							});
							break;
						case NbtString nbtString:
							result.Add(new BlockStateString()
							{
								Name = nbtString.Name,
								Value = nbtString.Value
							});
							break;
					}
				}
			}

			return result;
		}

		public static T FromNbt<T>(NbtTag tag) where T : Block
		{
			return (T) FromNbt(tag);
		}

		public static Block FromNbt(NbtTag compound)
		{
			// TODO - rework on serialization
			var id = compound["name"].StringValue;

			var states = new BlockStateContainer()
			{
				Id = id,
				States = GetBlockStates(compound)
			};

			if (BlockStates.TryGetValue(states, out var blockState))
			{
				return GetBlockByRuntimeId(blockState.RuntimeId);
			}

			return null;
		}

		public static string GetBlockIdFromItemId(string id)
		{
			return ItemToBlock.GetValueOrDefault(id);
		}

		public static string GetIdByType<T>(bool withRoot = true)
		{
			return GetIdByType(typeof(T), withRoot);
		}

		public static string GetIdByType(Type type, bool withRoot = true)
		{
			return withRoot 
				? TypeToId.GetValueOrDefault(type)
				: TypeToId.GetValueOrDefault(type).Replace("minecraft:", "");
		}

		public static string GetIdByRuntimeId(int id)
		{
			return RuntimeIdToId.GetValueOrDefault(id);
		}

		public static T GetBlockById<T>(string id, short metadata) where T : Block
		{
			return (T) GetBlockById(id, metadata);
		}

		public static Block GetBlockById(string id, short metadata)
		{
			var block = GetBlockById(id);

			if (!MetaBlockNameToState.TryGetValue(GetMetaBlockName(id, metadata), out var map))
			{
				return block;
			}

			block.SetState(map.States);
			block.Metadata = (byte) metadata;

			return block;
		}

		public static T GetBlockById<T>(string id) where T : Block
		{
			return (T) GetBlockById(id);
		}

		public static Block GetBlockById(string id)
		{
			if (string.IsNullOrEmpty(id)) return null;
			
			return IdToFactory.GetValueOrDefault(id)?.Invoke();
		}

		public static Block GetBlockByRuntimeId(int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return null;

			var blockState = BlockPalette[runtimeId];
			var block = GetBlockById(blockState.Id);

			if (block != null)
			{
				block.SetState(blockState.States);
				block.Metadata = (byte) blockState.Data;
			}

			return block;
		}

		public static bool IsBlock<T>(int runtimeId) where T : Block
		{
			return IsBlock(runtimeId, typeof(T));
		}

		public static bool IsBlock(int runtimeId, Type blockType)
		{
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return false;

			return IsBlock(BlockPalette[runtimeId].Id, blockType);
		}

		public static bool IsBlock<T>(string id) where T : Block
		{
			return IsBlock(id, typeof(T));
		}

		public static bool IsBlock(string id, Type blockType)
		{
			if (string.IsNullOrEmpty(id)) return false;

			var type = IdToType.GetValueOrDefault(id);

			return type.IsAssignableTo(blockType);
		}

		private static (Dictionary<string, Type>, Dictionary<Type, string>) BuildIdTypeMapPair()
		{
			var idToType = new Dictionary<string, Type>();
			var typeToId = new Dictionary<Type, string>();

			var blockTypes = typeof(BlockFactory).Assembly.GetTypes().Where(type => type.IsAssignableTo(typeof(Block)) && !type.IsAbstract);

			foreach (var type in blockTypes)
			{
				if (type == typeof(Block)) continue;

				var block = (Block) Activator.CreateInstance(type);
				var state = block.GetState();

				if (state == null) continue;

				idToType[state.Id] = type;
				typeToId[type] = state.Id;
			}

			return (idToType, typeToId);
		}

		private static Dictionary<string, Func<Block>> BuildIdToFactory()
		{
			var idToFactory = new Dictionary<string, Func<Block>>();

			foreach (var pair in IdToType)
			{
				// faster then Activator.CreateInstance
				var constructorExpression = Expression.New(pair.Value);
				var lambdaExpression = Expression.Lambda<Func<Block>>(constructorExpression);
				var createFunc = lambdaExpression.Compile();

				idToFactory.Add(pair.Key, createFunc);
			}

			return idToFactory;
		}

		private static (byte[], byte[]) BuildTransperentAndLuminousMapPair()
		{
			var transparentBlocks = new byte[BlockPalette.Count];
			var luminousBlocks = new byte[BlockPalette.Count];

			for (var i = 0; i < BlockPalette.Count; i++)
			{
				var block = GetBlockByRuntimeId(i);
				if (block != null)
				{
					if (block.IsTransparent)
					{
						transparentBlocks[i] = 1;
					}
					if (block.LightLevel > 0)
					{
						luminousBlocks[i] = (byte) block.LightLevel;
					}
				}
			}

			return (transparentBlocks, luminousBlocks);
		}

		private static Dictionary<int, string> BuildRuntimeIdToId()
		{
			var runtimeIdToId = new Dictionary<int, string>();

			for (var i = 0; i < BlockPalette.Count; i++)
			{
				runtimeIdToId.Add(i, BlockPalette[i].Id);
			}

			return runtimeIdToId;
		}

		private static string GetMetaBlockName(string name, short meta)
		{
			return $"{name}:{meta}";
		}
	}
}