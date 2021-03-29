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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using fNbt;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.BlockEntities
{
	public class ChalkboardBlockEntity : BlockEntity
	{
		public string Text { get; set; }
		public bool Locked { get; set; }
		public bool OnGround { get; set; }
		public long Owner { get; set; }
		public int Size { get; set; }
		public BlockCoordinates BaseCoordinates { get; set; }

		public ChalkboardBlockEntity() : base("ChalkboardBlock")
		{
			Text = string.Empty;
		}

		public override NbtCompound GetCompound()
		{
			NbtCompound compound;
			if (Coordinates == BaseCoordinates)
			{
				compound = new NbtCompound(string.Empty)
				{
					new NbtInt("BaseX", BaseCoordinates.X),
					new NbtInt("BaseY", BaseCoordinates.Y),
					new NbtInt("BaseZ", BaseCoordinates.Z),
					new NbtByte("Locked", (byte) (Locked ? 1 : 0)),
					new NbtByte("OnGround", (byte) (OnGround ? 1 : 0)),
					new NbtLong("Owner", Owner),
					new NbtInt("Size", Size),
					new NbtString("Text", Text ?? string.Empty),
					new NbtString("id", Id),
					new NbtInt("x", Coordinates.X),
					new NbtInt("y", Coordinates.Y),
					new NbtInt("z", Coordinates.Z)
				};
			}
			else
			{
				compound = new NbtCompound(string.Empty)
				{
					new NbtInt("BaseX", BaseCoordinates.X),
					new NbtInt("BaseY", BaseCoordinates.Y),
					new NbtInt("BaseZ", BaseCoordinates.Z),
					new NbtString("id", Id),
					new NbtInt("x", Coordinates.X),
					new NbtInt("y", Coordinates.Y),
					new NbtInt("z", Coordinates.Z)
				};
			}

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			if (compound.TryGet("Locked", out var locked)) Locked = locked.ByteValue == 1;
			if (compound.TryGet("OnGround", out var onGround)) OnGround = onGround.ByteValue == 1;
			if (compound.TryGet("Owner", out var owner)) Owner = owner.LongValue;
			if (compound.TryGet("Size", out var size)) Size = size.IntValue;

			int x = 0, y = 0, z = 0;
			if (compound.TryGet("BaseX", out var baseX)) x = baseX.IntValue;
			if (compound.TryGet("BaseY", out var baseY)) y = baseY.IntValue;
			if (compound.TryGet("BaseZ", out var baseZ)) z = baseZ.IntValue;
			BaseCoordinates = new BlockCoordinates(x, y, z);

			Text = GetTextValue(compound, "Text");
		}

		private string GetTextValue(NbtCompound compound, string key)
		{
			compound.TryGet(key, out NbtString text);
			return text != null ? (text.StringValue ?? string.Empty) : string.Empty;
		}
	}
}