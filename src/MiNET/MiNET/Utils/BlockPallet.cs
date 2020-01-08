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
				RootTag = (NbtList) pallet
			};
			byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);
			return nbtBinary;
		}
	}

	public class BlockRecord
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<BlockState> States { get; set; }

		public short Data { get; set; }
		public int RuntimeId { get; set; }

		public static explicit operator NbtCompound(BlockRecord record)
		{
			var states = new List<NbtTag>();
			foreach (BlockState state in record.States)
			{
				var stateTag = (NbtTag) state;
				if (stateTag == null) return null;

				states.Add(stateTag);
			}

			return new NbtCompound()
			{
				new NbtShort("id", (short) record.Data),
				new NbtShort("data", (short) record.Id),
				new NbtCompound("block")
				{
					new NbtString("name", record.Name),
					new NbtCompound("states", states),
					new NbtInt("version", BlockPallet.Version),
				}
			};
		}

		protected bool Equals(BlockRecord other)
		{
			bool result = Id == other.Id && Name == other.Name;
			if (!result) return false;

			var thisStates = new HashSet<BlockState>(this.States);
			var otherStates = new HashSet<BlockState>(other.States);

			otherStates.IntersectWith(thisStates);
			result = otherStates.Count == thisStates.Count;

			return result;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BlockRecord) obj);
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			hash.Add(Id);
			hash.Add(Name);
			foreach (var state in States) hash.Add(state);

			return hash.ToHashCode();
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
			if (state.Type == (byte) NbtTagType.Byte)
			{
				if (Byte.TryParse(state.Value, out byte value)) return new NbtByte(state.Name, value);
			}
			else if (state.Type == (byte) NbtTagType.Int)
			{
				if (Int32.TryParse(state.Value, out int value)) return new NbtInt(state.Name, value);
			}
			else if (state.Type == (byte) NbtTagType.String)
			{
				return new NbtString(state.Name, state.Value);
			}
			return null;
		}

		protected bool Equals(BlockState other)
		{
			return Type == other.Type && Name == other.Name && Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BlockState) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Name, Value);
		}
	}
}