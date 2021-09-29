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
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace MiNET.Utils
{
	public class BlockPalette : List<BlockStateContainer>
	{
		public static int Version => 17694723;

		public static BlockPalette FromJson(string json)
		{
			var pallet = new BlockPalette();

			dynamic result = JsonConvert.DeserializeObject<dynamic>(json);
			int runtimeId = 0;
			foreach (dynamic obj in result)
			{
				var record = new BlockStateContainer();
				record.Id = obj.Id;
				record.Name = obj.Name;
				record.Data = obj.Data;
				record.RuntimeId = runtimeId++;

				foreach (dynamic stateObj in obj.States)
				{
					switch ((int) stateObj.Type)
					{
						case 1:
						{
							record.States.Add(new BlockStateByte()
							{
								Name = stateObj.Name,
								Value = stateObj.Value
							});
							break;
						}
						case 3:
						{
							record.States.Add(new BlockStateInt()
							{
								Name = stateObj.Name,
								Value = stateObj.Value
							});
							break;
						}
						case 8:
						{
							record.States.Add(new BlockStateString()
							{
								Name = stateObj.Name,
								Value = stateObj.Value
							});
							break;
						}
					}
				}

				dynamic itemInstance = obj.ItemInstance;
				if (itemInstance != null)
				{
					record.ItemInstance = new ItemPickInstance()
					{
						Id = itemInstance.Id,
						Metadata = itemInstance.Metadata,
						WantNbt = itemInstance.WantNbt
					};
				}

				pallet.Add(record);
			}


			return pallet;
		}
	}

	public class BlockStateContainer
	{
		public int Id { get; set; }
		public short Data { get; set; }
		public string Name { get; set; }
		public int RuntimeId { get; set; }
		public List<IBlockState> States { get; set; } = new List<IBlockState>();

		[JsonIgnore]
		public byte[] StatesCacheNbt { get; set; }
		public ItemPickInstance ItemInstance { get; set; }

		protected bool Equals(BlockStateContainer other)
		{
			bool result = /*Id == other.Id && */Name == other.Name;
			if (!result) return false;

			var thisStates = new HashSet<IBlockState>(States);
			var otherStates = new HashSet<IBlockState>(other.States);

			otherStates.IntersectWith(thisStates);
			result = otherStates.Count == thisStates.Count;

			return result;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BlockStateContainer) obj);
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			hash.Add(Id);
			hash.Add(Name);
			foreach (var state in States)
			{
				switch (state)
				{
					case BlockStateByte blockStateByte:
						hash.Add(blockStateByte);
						break;
					case BlockStateInt blockStateInt:
						hash.Add(blockStateInt);
						break;
					case BlockStateString blockStateString:
						hash.Add(blockStateString);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(state));
				}
			}

			int hashCode = hash.ToHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Id)}: {Id}, {nameof(Data)}: {Data}, {nameof(RuntimeId)}: {RuntimeId}, {nameof(States)} {{ {string.Join(';', States)} }}";
		}
	}

	public class ItemPickInstance
	{
		public short Id { get; set; } = -1;
		public short Metadata { get; set; } = -1;
		public bool WantNbt { get; set; } = false;
	}

	public interface IBlockState
	{
		public string Name { get; set; }
	}

	public class BlockStateInt : IBlockState
	{
		public int Type { get; } = 3;
		public string Name { get; set; }
		public int Value { get; set; }

		protected bool Equals(BlockStateInt other)
		{
			return Name == other.Name && Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != this.GetType())
				return false;
			return Equals((BlockStateInt) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GetType().Name, Name, Value);
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
		}
	}

	public class BlockStateByte : IBlockState
	{
		public int Type { get; } = 1;
		public string Name { get; set; }
		public byte Value { get; set; }

		protected bool Equals(BlockStateByte other)
		{
			return Name == other.Name && Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != this.GetType())
				return false;
			return Equals((BlockStateByte) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GetType().Name, Name, Value);
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
		}
	}

	public class BlockStateString : IBlockState
	{
		public int Type { get; } = 8;
		public string Name { get; set; }
		public string Value { get; set; }

		protected bool Equals(BlockStateString other)
		{
			return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != this.GetType())
				return false;
			return Equals((BlockStateString) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GetType().Name, Name, Value.ToLowerInvariant());
		}

		public override string ToString()
		{
			return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
		}
	}
}