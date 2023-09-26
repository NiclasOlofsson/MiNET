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

using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public partial class ItemBucket : Item
	{
		public ItemBucket() : base()
		{
			MaxStackSize = 16;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// Pick up water/lava
			var block = world.GetBlock(blockCoordinates);
			switch (block)
			{
				case Stationary fluid:
					if (fluid.LiquidDepth == 0) // Only source blocks
					{
						switch (block)
						{
							case Lava:
								player.Inventory.AddItem(new ItemLavaBucket(), true);
								break;
							case Water:
								player.Inventory.AddItem(new ItemWaterBucket(), true);
								break;

							default: return false;
						}

						world.SetAir(blockCoordinates);
						Count--;
					}
					return true;
				case Flowing fluid:
					if (fluid.LiquidDepth == 0) // Only source blocks
					{
						switch (block)
						{
							case FlowingLava:
								player.Inventory.AddItem(new ItemLavaBucket(), true);
								break;
							case FlowingWater:
								player.Inventory.AddItem(new ItemWaterBucket(), true);
								break;

							default:
								return false;
						}

						world.SetAir(blockCoordinates);
						Count--;
					}
					return true;
			}

			return false;
		}
	}
}