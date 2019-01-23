﻿#region LICENSE

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

using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Cauldron : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Cauldron));

		public Cauldron() : base(118)
		{
			IsTransparent = true;
			BlastResistance = 10;
			Hardness = 2;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();

			if (itemInHand is ItemBucket)
			{
				if (itemInHand.Metadata == 8)
				{
					if (Metadata < 8)
					{
						world.SetData(Coordinates, 8);
						itemInHand.Metadata = 0;
						player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
					}
				}
				else if (itemInHand.Metadata == 0)
				{
					if (Metadata > 0)
					{
						world.SetData(Coordinates, 0);
						itemInHand.Metadata = 8;
						player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
					}
				}
			}

			return true; // Handled
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(380)};
		}
	}
}