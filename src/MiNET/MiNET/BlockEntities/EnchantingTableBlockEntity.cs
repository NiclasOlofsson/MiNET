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

namespace MiNET.BlockEntities
{
	public class EnchantingTableBlockEntity : BlockEntity
	{
		private NbtCompound Compound { get; set; }

		public EnchantingTableBlockEntity() : base("EnchantTable")
		{
			Compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtList("Items", new NbtCompound()),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			NbtList items = (NbtList) Compound["Items"];
			//for (byte i = 0; i < 2; i++)
			//{
			//	items.Add(new NbtCompound()
			//	{
			//		new NbtByte("Count", 0),
			//		new NbtByte("Slot", i),
			//		new NbtShort("id", 0),
			//		new NbtByte("Damage", 0),
			//	});
			//}

			items.Add(new NbtCompound()
			{
				new NbtByte("Count", 0),
				new NbtByte("Slot", 0),
				new NbtShort("id", 0),
				new NbtShort("Damage", 0),
			});
			items.Add(new NbtCompound()
			{
				new NbtByte("Count", 0),
				new NbtByte("Slot", 1),
				new NbtShort("id", 0),
				new NbtShort("Damage", 0),
			});
		}

		public override NbtCompound GetCompound()
		{
			Compound["x"] = new NbtInt("x", Coordinates.X);
			Compound["y"] = new NbtInt("y", Coordinates.Y);
			Compound["z"] = new NbtInt("z", Coordinates.Z);

			return Compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Compound = compound;

			if (Compound["Items"] == null)
			{
				NbtList items = new NbtList("Items");
				for (byte i = 0; i < 2; i++)
				{
					items.Add(new NbtCompound()
					{
						new NbtByte("Count", 0),
						new NbtByte("Slot", i),
						new NbtShort("id", 0),
						new NbtShort("Damage", 0),
					});
				}
				Compound["Items"] = items;
			}
		}
	}
}