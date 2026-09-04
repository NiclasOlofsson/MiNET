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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using fNbt;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Blocks
{
	/// <summary>
	///     Replaces the implementation of a vanilla block: return your own class for a name and the
	///     server builds that instead, keeping the block's identity and palette entry. Asked by name
	///     because that is a block's identity after the flattening, and consulted from
	///     <see cref="BlockFactory.GetBlockByName" />, which every other lookup ends at, so the
	///     substitution holds whether the block came from an item, a recipe or a world load. Return
	///     null for a name you are not replacing.
	///     The state is applied to whatever comes back, so the replacement has to handle it or the
	///     block cannot be saved or sent: derive from the vanilla class and it is handled already,
	///     or override <see cref="Block.SetState(List{IBlockState})" /> and
	///     <see cref="Block.GetState" /> yourself. Branching on the state belongs in SetState, which
	///     is why replacing a block is by name and not per state.
	/// </summary>
	public interface ICustomBlockFactory
	{
		Block GetBlockByName(string name);
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

		// Built on first use, not in the static constructor: it reads the palette, and the palette is
		// assigned later in Init.
		private static readonly Lazy<Dictionary<string, int>> _nameToId = new Lazy<Dictionary<string, int>>(BuildNameToId);

		[Obsolete("Keyed on a fuzzy name and answers a legacy numeric id, which post-flattening most blocks do not have. Use GetDefaultState(name).RuntimeId.")]
		public static Dictionary<string, int> NameToId => _nameToId.Value;

		/// <summary>
		///     Block identity by its Minecraft name, which is what the palette, the wire and items all
		///     use. The legacy numeric id predates flattening: one id covered every wood type, every
		///     colour, and the variant lived in a data value. Looking a block up by that id hands back
		///     the pre-flattening class, whose state does not exist in the palette any more, so the
		///     block cannot be written to the world. Anything that is not translating stored data
		///     should come through here.
		/// </summary>
		private static Dictionary<string, Type> NameToBlockType { get; set; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
		public static BlockPalette BlockPalette { get; set; } = null;
		public static HashSet<BlockStateContainer> BlockStates { get; set; } = null;

		// runtime id -> FNV-1a 32 network hash of the block state. This is the order-independent
		// block id form used on the wire when StartGame.blockNetworkIdsAreHashes is true (the
		// vanilla BDS default since 1.19.80); hashing over {name, states} makes chunk block ids
		// immune to palette-order differences between server and client. minecraft:unknown is
		// hardcoded to -2 by the vanilla implementation.
		private static readonly Lazy<uint[]> _networkHashes = new Lazy<uint[]>(() =>
		{
			var hashes = new uint[BlockPalette.Count];
			var taken = new HashSet<uint>(BlockPalette.Count);
			for (int i = 0; i < BlockPalette.Count; i++)
			{
				// A colliding state takes the next free value upwards, resolved walking the palette
				// in canonical order, which is what the client does. Resolving differently would
				// hand the client the id of the block it collided with. The pinned palette has no
				// collisions (16913 states, birthday expectation 0.03), so this changes nothing
				// today and exists for the version where it does.
				uint hash = ComputeNetworkHash(BlockPalette[i]);
				while (!taken.Add(hash)) hash++;
				hashes[i] = hash;
			}
			return hashes;
		});

		private static readonly Lazy<Dictionary<uint, int>> _runtimeIdByHash = new Lazy<Dictionary<uint, int>>(() =>
		{
			uint[] hashes = _networkHashes.Value;
			var map = new Dictionary<uint, int>(hashes.Length);
			for (int i = 0; i < hashes.Length; i++)
			{
				map[hashes[i]] = i;
			}
			return map;
		});

		/// <summary>
		///     Which of the two forms the single protocol "block runtime id" field carries. There is
		///     one such field, never two: this only decides what number goes in it. StartGame tells
		///     the client which, and after that the whole session reads it one way.
		///     False, the vanilla-incompatible but cheaper form, means the raw palette index, which
		///     works because our palette is compiled in the client's own order. True means the
		///     order-independent FNV-1a hash of the state, which costs about four extra bytes per
		///     sub-chunk palette entry.
		/// </summary>
		public static bool BlockNetworkIdsAreHashes { get; set; } = Config.GetProperty("BlockNetworkIdsAreHashes", false);

		/// <summary>
		///     Internal identity to the wire. The ONLY place that turns a block into the number the
		///     protocol carries: nothing else may look at <see cref="BlockNetworkIdsAreHashes" />.
		/// </summary>
		public static uint GetNetworkId(int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return 0;

			return BlockNetworkIdsAreHashes ? _networkHashes.Value[runtimeId] : (uint) runtimeId;
		}

		public static uint GetNetworkId(Block block)
		{
			return GetNetworkId(block.GetRuntimeId());
		}

		public static uint GetNetworkId(BlockStateContainer state)
		{
			return GetNetworkId(state.RuntimeId);
		}

		/// <summary>
		///     The wire back to internal identity, and the only inverse of <see cref="GetNetworkId" />.
		///     Returns -1 for a number in neither form, which is a protocol error, not air.
		/// </summary>
		public static int GetRuntimeIdFromNetworkId(uint networkId)
		{
			if (BlockNetworkIdsAreHashes)
			{
				return _runtimeIdByHash.Value.TryGetValue(networkId, out int runtimeId) ? runtimeId : -1;
			}

			return networkId < BlockPalette.Count ? (int) networkId : -1;
		}

		public static Block GetBlockByNetworkId(uint networkId)
		{
			int runtimeId = GetRuntimeIdFromNetworkId(networkId);
			return runtimeId < 0 ? null : GetBlockByRuntimeId(runtimeId);
		}

		/// <summary>
		///     One block per runtime id, stated, built once. Every characteristic the hot paths ask
		///     for is a constant of the class and its state, so a prototype answers it by reading a
		///     field instead of materializing a block per query: about 4ns and no allocation, against
		///     tens of nanoseconds and 120 bytes through <see cref="GetBlockByRuntimeId" />.
		///     Keyed by runtime id rather than legacy id, so a block without a legacy id still has an
		///     answer, and leaves, water and cobweb need no naming as exceptions. Stated rather than
		///     resolved per name, so a characteristic that varies with state (an open door, a top
		///     slab) is a separate entry and no caller needs a special case for it.
		///     Private, and characteristics leave here as values, never as the object: a Block carries
		///     position, and its Coordinates, light and biome are written per query, so a shared
		///     instance that escaped would be cross-talk between every block at that runtime id.
		/// </summary>
		private static readonly Lazy<Block[]> _prototypes = new Lazy<Block[]>(() =>
		{
			var prototypes = new Block[BlockPalette.Count];
			for (int i = 0; i < BlockPalette.Count; i++) prototypes[i] = GetBlockByRuntimeId(i);
			return prototypes;
		});

		/// <summary>The shared block for a runtime id, never handed out and never written to. Null if the id is unknown.</summary>
		private static Block Prototype(int runtimeId)
		{
			Block[] prototypes = _prototypes.Value;
			return runtimeId >= 0 && runtimeId < prototypes.Length ? prototypes[runtimeId] : null;
		}

		/// <summary>
		///     Whether the block at a runtime id is a <typeparamref name="T" />, by class test on the
		///     stated prototype: one array read and an isinst, inheritance included, no block built.
		///     This is the probe form for hot paths; <see cref="GetBlockByRuntimeId" /> stays for
		///     callers that need an instance to interact with.
		/// </summary>
		public static bool Is<T>(int runtimeId) where T : Block
		{
			return Prototype(runtimeId) is T;
		}

		/// <summary>Whether skylight passes through the block undimmed.</summary>
		public static bool SkyLightPasses(int runtimeId)
		{
			return Prototype(runtimeId)?.LightDampening == 0;
		}

		/// <summary>
		///     What the skylight pass subtracts crossing the block: one for the step, plus whatever the
		///     block itself filters. The pass asks this per block per column.
		/// </summary>
		public static byte GetLightDiffusion(int runtimeId)
		{
			return (byte) Math.Min(15, 1 + (Prototype(runtimeId)?.LightDampening ?? 0));
		}

		/// <summary>Light this block emits, 0 to 15.</summary>
		public static byte GetLightEmission(int runtimeId)
		{
			return (byte) (Prototype(runtimeId)?.LightLevel ?? 0);
		}

		/// <summary>Whether the block is see-through at all, which is not the same as passing light undimmed.</summary>
		public static bool IsTransparent(int runtimeId)
		{
			return Prototype(runtimeId)?.IsTransparent ?? false;
		}

		/// <summary>
		///     Whether the block is a full solid cube. This is NOT the same question as whether it
		///     stops movement: a fence and a closed door are both IsSolid false and both stop a mob.
		///     Ask <see cref="BlocksMovement" /> for that.
		///     An id with no block is nothing standing there, so nothing solid.
		/// </summary>
		public static bool IsSolid(int runtimeId)
		{
			return Prototype(runtimeId)?.IsSolid ?? false;
		}


		/// <summary>
		///     Air is stateless, so it holds a single palette entry and being air is one integer
		///     comparison against its runtime id. The numeric block id answers the same question,
		///     but only after projecting the palette entry onto Bedrock's id map, which is a lookup
		///     between the caller and a comparison it could have made directly.
		/// </summary>
		private static readonly Lazy<int> _airRuntimeId = new Lazy<int>(() => GetDefaultState("minecraft:air").RuntimeId);

		public static int AirRuntimeId => _airRuntimeId.Value;

		public static bool IsAir(int runtimeId)
		{
			return runtimeId == _airRuntimeId.Value;
		}

		/// <summary>Which block sits at a runtime id, without building one. Null if the id is unknown.</summary>
		public static string GetBlockName(int runtimeId)
		{
			return runtimeId >= 0 && runtimeId < BlockPalette.Count ? BlockPalette[runtimeId].Name : null;
		}

		// A block name's default (first palette) state; null if the name is unknown. Callers that
		// want a wire id take this to GetNetworkId, so the id form stays decided in one place.
		public static BlockStateContainer GetDefaultState(string name)
		{
			if (string.IsNullOrEmpty(name)) return null;
			if (!name.StartsWith("minecraft:")) name = "minecraft:" + name;
			return _defaultStateByName.Value.TryGetValue(name, out var state) ? state : null;
		}

		// FNV-1a 32 over the standard little-endian (non-varint) NBT of {name, states}, states
		// sorted alphabetically by name. Mirrors MiNET.Client NetworkBlockPalette.ComputeNetworkHash,
		// which is verified against live BDS 1.26.34 chunk data.
		public static uint ComputeNetworkHash(BlockStateContainer container)
		{
			if (container.Name == "minecraft:unknown") return unchecked((uint) -2);

			byte[] bytes = SerializeHashDocument(container);

			uint hash = 0x811c9dc5;
			foreach (byte b in bytes)
			{
				hash ^= b;
				hash *= 0x01000193;
			}

			return hash;
		}

		private static byte[] SerializeHashDocument(BlockStateContainer container)
		{
			var statesCompound = new NbtCompound("states");
			foreach (IBlockState state in container.States.OrderBy(s => s.Name, StringComparer.Ordinal))
			{
				switch (state)
				{
					case BlockStateByte b:
						statesCompound.Add(new NbtByte(b.Name, b.Value));
						break;
					case BlockStateInt i:
						statesCompound.Add(new NbtInt(i.Name, i.Value));
						break;
					case BlockStateString s:
						statesCompound.Add(new NbtString(s.Name, s.Value));
						break;
				}
			}

			var root = new NbtCompound("")
			{
				new NbtString("name", container.Name),
				statesCompound
			};

			var file = new NbtFile
			{
				BigEndian = false,
				UseVarInt = false,
				RootTag = root
			};

			return file.SaveToBuffer(NbtCompression.None);
		}

		public static int[] LegacyToRuntimeId = new int[65536];

		static BlockFactory()
		{
			NameToBlockType = BuildNameToBlockType();

			for (int i = 0; i < LegacyToRuntimeId.Length; ++i)
			{
				LegacyToRuntimeId[i] = -1;
			}

			var assembly = Assembly.GetAssembly(typeof(Block));

			lock (lockObj)
			{
				Dictionary<string, int> idMapping = new Dictionary<string, int>(ResourceUtil.ReadResource<Dictionary<string, int>>("block_id_map.json", typeof(Block), "Data"), StringComparer.OrdinalIgnoreCase);
				Dictionary<string, int> itemIdMapping = new Dictionary<string, int>(ResourceUtil.ReadResource<Dictionary<string, int>>("item_id_map.json", typeof(Item), "Data"), StringComparer.OrdinalIgnoreCase);

				// The palette is compiled in rather than parsed from NBT at startup. It is an
				// ordered list whose index IS the network id, and all of that is known when the
				// code is generated, so there is nothing to work out here. Regenerate with
				// MiNET.BlockGen after moving the pinned data submodule.
				BlockPalette = new BlockPalette();
				BlockPaletteData.Create(BlockPalette);

				List<R12ToCurrentBlockMapEntry> legacyStateMap = new List<R12ToCurrentBlockMapEntry>();
				using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".Data.r12_to_current_block_map.bin"))
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

						legacyStateMap.Add(new R12ToCurrentBlockMapEntry(stringId, meta, GetBlockStateContainer(compound)));
					}
				}
				
				Dictionary<string, List<int>> idToStatesMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

				for (var index = 0; index < BlockPalette.Count; index++)
				{
					var state = BlockPalette[index];
					List<int> candidates;

					if (!idToStatesMap.TryGetValue(state.Name, out candidates))
						candidates = new List<int>();

					candidates.Add(index);

					idToStatesMap[state.Name] = candidates;
				}

				foreach (var pair in legacyStateMap)
				{
					if (!idMapping.TryGetValue(pair.StringId, out int id))
						continue;

					var data = pair.Meta;

					if (data > 15)
					{
						continue;
					}

					var mappedState = pair.State;
					var mappedName = pair.State.Name;

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
							BlockPalette[match].Id = id;
							BlockPalette[match].Data = data;

							// Blocks whose item form has its own id (doors, beds, ...) must pick as the item, not the block
							BlockPalette[match].ItemInstance = new ItemPickInstance()
							{
								Id = (short) (itemIdMapping.TryGetValue(networkState.Name, out int pickItemId) ? pickItemId : id),
								Metadata = data,
								WantNbt = false
							};

							LegacyToRuntimeId[(id << 4) | (byte) data] = match;

							break;
						}
					}
				}

				// Blocks added after the R12 legacy map (chain, blackstone, candles, ...) never match above.
				// Fall back to name-based lookups so GetBlockById() and block picking still work for them.
				foreach (var state in BlockPalette)
				{
					if (state.Id != 0 || state.Name == "minecraft:air") continue;

					if (idMapping.TryGetValue(state.Name, out int legacyId))
					{
						state.Id = legacyId;
					}

					if (state.ItemInstance == null && itemIdMapping.TryGetValue(state.Name, out int itemId))
					{
						state.ItemInstance = new ItemPickInstance()
						{
							Id = (short) itemId,
							Metadata = 0,
							WantNbt = false
						};
					}
				}

				foreach(var record in BlockPalette)
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

					var nbt = new NbtFile()
					{
						BigEndian = false,
						UseVarInt = true,
						RootTag = new NbtCompound("states", states)
					};

					byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);

					record.StatesCacheNbt = nbtBinary;
				}
			}
			
			BlockStates = new HashSet<BlockStateContainer>(BlockPalette);

			// Identity is the state hash alone, so two palette entries sharing one would be
			// indistinguishable and one of them unreachable. At 64 bits that is a 1-in-10^11 event
			// for a palette this size, which is exactly the kind of odds that eventually happen to
			// someone, so it is checked rather than assumed.
			if (BlockStates.Count != BlockPalette.Count)
			{
				Log.Error($"Block palette hash collision: {BlockPalette.Count} states collapsed to {BlockStates.Count}. Blocks are now unreachable and chunks will read wrong.");
			}
		}
		
		private static BlockStateContainer GetBlockStateContainer(NbtTag tag)
		{
			var record = new BlockStateContainer();

			string name = tag["name"].StringValue;
			record.Name = name;
			record.States = GetBlockStates(tag);

			return record;
		}

		private static List<IBlockState> GetBlockStates(NbtTag tag)
		{
			var result = new List<IBlockState>();

			var states = tag["states"];
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

		private static object lockObj = new object();

		/// <summary>
		///     Every Block subclass that can be constructed, keyed on the name it reports. Generated
		///     classes win over hand-written ones claiming the same name: the generated set is the
		///     one that matches the current palette.
		/// </summary>
		private static Dictionary<string, Type> BuildNameToBlockType()
		{
			var map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			foreach (Type type in Assembly.GetAssembly(typeof(Block)).GetTypes())
			{
				if (type.IsAbstract || !typeof(Block).IsAssignableFrom(type)) continue;
				if (type.GetConstructor(Type.EmptyTypes) == null) continue;

				Block block;
				try
				{
					block = (Block) Activator.CreateInstance(type);
				}
				catch (Exception e)
				{
					Log.Debug($"Could not construct block type {type.Name} for the name map", e);
					continue;
				}

				if (string.IsNullOrEmpty(block?.Name)) continue;

				if (map.ContainsKey(block.Name) && !block.IsGenerated) continue;

				map[block.Name] = type;
			}

			return map;
		}

		/// <summary>
		///     Loose block name to legacy id, for callers holding a name a user typed. Built from the
		///     palette, so a name that is not a block any more has no entry rather than resolving to a
		///     pre-flattening class.
		/// </summary>
		private static Dictionary<string, int> BuildNameToId()
		{
			var nameToId = new Dictionary<string, int>();
			foreach (BlockStateContainer state in BlockPalette)
			{
				if (state.Id <= 0) continue;

				nameToId.TryAdd(NormalizeBlockName(state.Name), state.Id);
			}

			return nameToId;
		}

		private static string NormalizeBlockName(string blockName)
		{
			return blockName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");
		}

		[Obsolete("Answers a legacy numeric id, which post-flattening most blocks do not have, and matches on a name stripped of its namespace and underscores. Use GetDefaultState(name).RuntimeId.")]
		public static int GetBlockIdByName(string blockName)
		{
			return NameToId.TryGetValue(NormalizeBlockName(blockName), out int id) ? id : 0;
		}

		// Palette-name resolution without legacy ids: the first palette state for a name is its
		// default state (canonical palette order). Covers every current block, including the
		// post-flattening ones that never had a legacy id.
		private static readonly Lazy<Dictionary<string, BlockStateContainer>> _defaultStateByName = new Lazy<Dictionary<string, BlockStateContainer>>(() =>
		{
			var result = new Dictionary<string, BlockStateContainer>(StringComparer.OrdinalIgnoreCase);
			foreach (BlockStateContainer state in BlockPalette)
			{
				result.TryAdd(state.Name, state);
			}
			return result;
		});

		/// <summary>
		///     The block for a Minecraft name, e.g. minecraft:oak_planks. This is the lookup to use:
		///     it returns the class whose state round-trips through the palette.
		/// </summary>
		public static Block GetBlockByName(string blockName)
		{
			if (string.IsNullOrEmpty(blockName)) return null;

			if (!blockName.Contains(':')) blockName = "minecraft:" + blockName;

			// The plugin gets first refusal, because a custom block factory is an override: it can
			// replace a block the palette owns, not only add names of its own.
			Block custom = CustomBlockFactory?.GetBlockByName(blockName);
			if (custom != null) return custom;

			return NameToBlockType.TryGetValue(blockName, out Type type) ? (Block) Activator.CreateInstance(type) : null;
		}

		/// <summary>
		///     The block for a palette index, with its state applied. Goes by name, so the block that
		///     comes back can be asked for its state again and resolve to the same runtime id.
		/// </summary>
		public static Block GetBlockByRuntimeId(int runtimeId)
		{
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return null;

			BlockStateContainer blockState = BlockPalette[runtimeId];

			Block block = GetBlockByName(blockState.Name);
			if (block == null)
			{
				Log.Warn($"No block class for palette name {blockState.Name} (runtime id {runtimeId})");
				return null;
			}

			block.SetState(blockState.States);
			return block;
		}

		/// <summary>
		///     Translates a stored legacy id and data value to the block it means now: id and data
		///     to a runtime id through the R12 table, then to the typed class that owns that palette
		///     entry. A pair the table does not cover ends at minecraft:info_update, which is what a
		///     client shows for a block it does not know. This is for reading old worlds; anything
		///     else should ask by name.
		/// </summary>
		public static Block GetBlockById(int blockId, byte metadata = 0)
		{
			return GetBlockByRuntimeId((int) GetRuntimeId(blockId, metadata));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetRuntimeId(int blockId, byte metadata)
		{
			int idx = TryGetRuntimeId(blockId, metadata);
			if (idx != -1)
			{
				return (uint) idx;
			}

			//block found with bad metadata, try getting with zero
			int idx2 = TryGetRuntimeId(blockId, 0);
			if (idx2 != -1)
			{
				return (uint) idx2;
			}

			return (uint) TryGetRuntimeId(248, 0); //legacy id for info_update block (for unknown block)
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int TryGetRuntimeId(int blockId, byte metadata)
		{
			// (blockId << 4) leaves the table long before an int overflows, and a stored id is
			// whatever the old world happened to hold.
			int index = (blockId << 4) | metadata;
			return index >= 0 && index < LegacyToRuntimeId.Length ? LegacyToRuntimeId[index] : -1;
		}
	}
}