using System;
using System.Collections.Generic;
using System.Linq;
using fNbt;
using Newtonsoft.Json;

namespace MiNET.Utils
{
	public class BlockPallet : List<BlockRecord>
	{
		public static int Version => 17694723;

		public static BlockPallet FromJson(string json)
		{
			BlockPallet pallet = JsonConvert.DeserializeObject<BlockPallet>(json);
			int runtimeId = 0;
			foreach (BlockRecord record in pallet)
			{
				record.RuntimeId = runtimeId;
				runtimeId++;
			}
			return pallet;
		}

		public static explicit operator NbtList(BlockPallet pallet)
		{
			var list = new NbtList("", NbtTagType.Compound);
			foreach (BlockRecord record in pallet)
			{
				var recordTag = (NbtCompound) record;
				if (recordTag == null) return null;

				list.Add(recordTag);
			}
			return list;
		}

		public static explicit operator byte[](BlockPallet pallet)
		{
			// huge duck tape
			// empty name of compound is important
			var nbt = new NbtFile()
			{
				BigEndian = false,
				UseVarInt = true,
				RootTag = new NbtCompound("")
				{
					(NbtList) pallet
				}
			};
			byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);
			if (nbtBinary.Length > 3) // 0a0000 - empty compound with empty name
			{
				byte[] result = new byte[nbtBinary.Length - 3];
				Array.Copy(nbtBinary, 2, result, 0, result.Length);
				return result;
			}
			return new byte[0];
		}
	}

	public class BlockRecord
	{
		public int Id { get; set; }
		public short Data { get; set; }
		public string Name { get; set; }
		public List<BlockState> States { get; set; }

		public int RuntimeId { get; set; }

		public static explicit operator NbtCompound(BlockRecord record)
		{
			var states = new List<NbtTag>();
			foreach (BlockState state in record.States)
			{
				var stateTag = (NbtTag)state;
				if (stateTag == null) return null;

				states.Add(stateTag);
			}

			return new NbtCompound()
			{
				new NbtCompound("block")
				{
					new NbtString("name", record.Name),
					new NbtCompound("states", states),
					new NbtInt("version", BlockPallet.Version),
				},
				new NbtShort("id", (short) record.Id)
			};
		}
	}

	public class BlockState
	{
		public byte Type { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }

		// BlockState can be an only simple type (for 1.13 it is an only byte, int or string)
		public static explicit operator NbtTag(BlockState state)
		{
			if (state.Type == (byte)NbtTagType.Byte)
			{
				if (Byte.TryParse(state.Value, out byte value)) return new NbtByte(state.Name, value);
			}
			else if (state.Type == (byte)NbtTagType.Int)
			{
				if (Int32.TryParse(state.Value, out int value)) return new NbtInt(state.Name, value);
			}
			else if (state.Type == (byte)NbtTagType.String)
			{
				return new NbtString(state.Name, state.Value);
			}
			return null;
		}
	}
}
