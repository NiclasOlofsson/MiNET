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
using log4net;
using MiNET.Blocks;
using MiNET.Items;

namespace MiNET.BlockEntities
{
	public class FlowerPotBlockEntity : BlockEntity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(FlowerPotBlockEntity));

		public Block PlantBlock { get; set; }

		public FlowerPotBlockEntity() : base("FlowerPot")
		{
		}

		public override NbtCompound GetCompound()
		{
			var compaund = new NbtCompound(string.Empty) 
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			if (PlantBlock != null)
			{
				compaund.Add(PlantBlock.ToNbt("PlantBlock"));
			}

			return compaund;
		}

		public override void SetCompound(NbtCompound compound)
		{
			var plantBlockTag = compound["PlantBlock"];
			if (plantBlockTag != null)
			{
				PlantBlock = BlockFactory.FromNbt(plantBlockTag);
			}
		}

		public override List<Item> GetDrops()
		{
			if (PlantBlock == null)
			{
				return new List<Item>();
			}

			return new List<Item> { ItemFactory.GetItem(PlantBlock) };
		}
	}
}