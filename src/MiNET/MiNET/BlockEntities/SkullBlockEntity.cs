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

using System.Collections.Generic;
using fNbt;
using MiNET.Items;

namespace MiNET.BlockEntities
{
	public class SkullBlockEntity : BlockEntity
	{
		public byte Rotation { get; set; }
		public byte SkullType { get; set; }

		public SkullBlockEntity() : base("Skull")
		{
		}

		//TAG_Compound: 9 entries {
		//	TAG_Byte("MouthMoving"): 0
		//	TAG_Int("MouthTickCount"): 0
		//	TAG_Byte("Rot"): 13
		//	TAG_Byte("SkullType"): 0
		//	TAG_String("id"): "Skull"
		//	TAG_Byte("isMovable"): 1
		//	TAG_Int("x"): -1
		//	TAG_Int("y"): 4
		//	TAG_Int("z"): -4
		//}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtByte("SkullType", SkullType),
				new NbtByte("Rot", Rotation)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			SkullType = compound["SkullType"].ByteValue;
			Rotation = compound["Rot"].ByteValue;
		}

		public override List<Item> GetDrops()
		{
			return new List<Item> {ItemFactory.GetItem(397, SkullType, 1)};
		}
	}
}