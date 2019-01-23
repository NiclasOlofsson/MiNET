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

using System.Collections.Generic;
using fNbt;

namespace MiNET.BlockEntities
{
	public class BannerBlockEntity : BlockEntity
	{
		public int Base { get; set; }
		public List<BannerPattern> Patterns { get; set; } = new List<BannerPattern>();

		public BannerBlockEntity() : base("Banner")
		{
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtString("id", Id),
			};

			if (Patterns.Count > 0)
			{
				NbtList items = new NbtList("Patterns", new NbtCompound());
				foreach (var pattern in Patterns)
				{
					items.Add(new NbtCompound
					{
						new NbtString("Pattern", pattern.Pattern),
						new NbtInt("Color", pattern.Color),
					});
				}
				compound.Add(items);
			}

			compound.Add(new NbtInt("Base", Base));

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			if (compound == null) return;

			Patterns.Clear();

			var baseColor = compound["Base"];
			if (baseColor != null)
			{
				Base = baseColor.IntValue;
			}

			NbtList items = (NbtList) compound["Patterns"];
			if (items != null)
			{
				foreach (var item in items)
				{
					Patterns.Add(new BannerPattern()
					{
						Pattern = item["Pattern"].StringValue,
						Color = item["Color"].IntValue
					});
				}
			}
		}
	}

	public class BannerPattern
	{
		public string Pattern { get; set; }
		public int Color { get; set; }
	}
}