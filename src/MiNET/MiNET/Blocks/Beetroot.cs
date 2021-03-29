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
using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Beetroot : Crops
	{
		public Beetroot() : base(244)
		{
			MaxGrowth = 4;
		}

		public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();
			if (Growth < MaxGrowth && itemInHand is ItemDye && itemInHand.Metadata == 15 && new Random().NextDouble() > 0.25)
			{
				Growth++;
				level.SetBlock(this);

				return true;
			}

			return false;
		}


		public override Item[] GetDrops(Item tool)
		{
			if (Growth == MaxGrowth)
			{
				// Can also return 0-3 seeds at random.
				var rnd = new Random();
				var count = rnd.Next(4);
				if (count > 0)
				{
					return new[] {ItemFactory.GetItem(457, 0, 1), ItemFactory.GetItem(458, 0, (byte) count)};
				}
				return new[] {ItemFactory.GetItem(457, 0, 1)};
			}

			return new[] {ItemFactory.GetItem(458, 0, 1)};
		}
	}
}