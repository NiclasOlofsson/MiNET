using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using fNbt;
using log4net;
using MiNET.Items;
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

		public static Dictionary<string, R12ToCurrentBlockMapEntry> NameToBlockMapEntry { get; private set; } = new Dictionary<string, R12ToCurrentBlockMapEntry>();
		public static Dictionary<int, string> RuntimeIdToId { get; private set; }
		public static Dictionary<string, Type> IdToType { get; private set; } = new Dictionary<string, Type>();
		public static Dictionary<Type, string> TypeToId { get; private set; } = new Dictionary<Type, string>();
		public static Dictionary<string, string> ItemToBlock { get; private set; }

		public static BlockPalette BlockPalette { get; set; }
		public static HashSet<BlockStateContainer> BlockStates { get; set; }

		private static readonly object _lockObj = new object();

		static BlockFactory()
		{
			var assembly = Assembly.GetAssembly(typeof(Block));

			lock (_lockObj)
			{
				int runtimeId = 0;
				BlockPalette = new BlockPalette();

				using (var stream = assembly.GetManifestResourceStream(typeof(BlockFactory).Namespace + ".Data.canonical_block_states.nbt"))
				{
					do
					{
						var compound = Packet.ReadNbtCompound(stream, true);
						var container = GetBlockStateContainer(compound);

						container.RuntimeId = runtimeId++;
						BlockPalette.Add(container);
					} while (stream.Position < stream.Length);
				}

				var visitedContainers = new HashSet<BlockStateContainer>();
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
						//if (!visitedContainers.TryGetValue(state, out _))
						{
							NameToBlockMapEntry.Add(GetMetaBlockName(stringId, meta), new R12ToCurrentBlockMapEntry(stringId, meta, state));
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

				foreach (var pair in NameToBlockMapEntry.Values)
				{
					var data = pair.Meta;

					var mappedState = pair.State;
					var mappedName = pair.State.Id;

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

							BlockPalette[match].ItemInstance = new ItemPickInstance()
							{
								Id = blockIdItemIdMap.GetValueOrDefault(mappedName, mappedName),
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
				}

				BlockStates = new HashSet<BlockStateContainer>(BlockPalette);

				(IdToType, TypeToId) = BuildIdTypeMapPair();
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

		public static Block FromNbt(NbtTag tag)
		{
			return FromNbt<Block>(tag);
		}

		public static Block FromNbt<T>(NbtTag compound) where T : Block
		{
			// TODO - rework on serialization
			var id = compound["name"].StringValue;
			var statesTag = compound["states"] as NbtList;

			var states = new BlockStateContainer()
			{
				Id = id
			};

			foreach (var stateTag in statesTag)
			{
				IBlockState state = null;
				switch (stateTag)
				{
					case NbtByte blockStateByte:
						state = new BlockStateByte()
						{
							Name = stateTag.Name, 
							Value = blockStateByte.Value
						};
						break;
					case NbtInt blockStateInt:
						state = new BlockStateInt()
						{
							Name = stateTag.Name,
							Value = blockStateInt.Value
						};
						break;
					case NbtString blockStateString:
						state = new BlockStateString()
						{
							Name = stateTag.Name,
							Value = blockStateString.Value
						};
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(stateTag));
				}

				states.States.Add(state);
			}

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

		public static string GetIdByType<T>()
		{
			return GetIdByType(typeof(T));
		}

		public static string GetIdByType(Type type)
		{
			return TypeToId.GetValueOrDefault(type);
		}

		public static string GetIdByRuntimeId(int id)
		{
			return RuntimeIdToId.GetValueOrDefault(id);
		}

		public static Block GetBlockById(string id)
		{
			return GetBlockById<Block>(id);
		}

		public static Block GetBlockById(string id, short metadata)
		{
			return GetBlockById<Block>(id, metadata);
		}

		public static T GetBlockById<T>(string id, short metadata) where T : Block
		{
			var block = GetBlockById<T>(id);

			if (!NameToBlockMapEntry.TryGetValue(GetMetaBlockName(id, metadata), out var map))
			{
				return block;
			}

			block.SetState(map.State);
			block.Metadata = (byte) metadata;

			return block;
		}

		public static T GetBlockById<T>(string id) where T : Block
		{
			if (string.IsNullOrEmpty(id)) return null;

			var type = IdToType.GetValueOrDefault(id);

			if (type == null) return null;

			return (T) Activator.CreateInstance(type);
		}

		public static Block GetBlockByRuntimeId(int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return null;

			var blockState = BlockPalette[runtimeId];
			var block = GetBlockById(blockState.Id);
			block.SetState(blockState.States);
			block.Metadata = (byte) blockState.Data;

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