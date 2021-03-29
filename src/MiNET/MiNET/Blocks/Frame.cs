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
using log4net;
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Frame : Block
	{
		public Frame() : base(199)
		{
			IsTransparent = true;
			IsSolid = false;
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplaceable;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			FacingDirection = (int) face;

			var itemFrameBlockEntity = new ItemFrameBlockEntity {Coordinates = Coordinates};
			world.SetBlockEntity(itemFrameBlockEntity);

			return false;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(Frame));

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Item itemInHand = player.Inventory.GetItemInHand();

			if (world.GetBlockEntity(blockCoordinates) is ItemFrameBlockEntity blockEntity)
			{
				int rotation = blockEntity.Rotation;

				if (itemInHand.Equals(blockEntity.ItemInFrame) && itemInHand.Equals(new ItemAir())) return true;

				if (itemInHand.Equals(blockEntity.ItemInFrame) || itemInHand.Equals(new ItemAir()))
				{
					itemInHand = blockEntity.ItemInFrame;
					rotation++;
					if (rotation > 7)
					{
						rotation = 0;
					}
				}
				else
				{
					rotation = 0;
				}

				blockEntity.SetItem(itemInHand, rotation);
				world.SetBlockEntity(blockEntity);
			}

			return true;
		}

		public void ClearItem(Level world)
		{
			if (world.GetBlockEntity(Coordinates) is ItemFrameBlockEntity blockEntity)
			{
				Item item = blockEntity.ItemInFrame;
				blockEntity.SetItem(null, 0);
				world.SetBlockEntity(blockEntity);
				if (item != null)
				{
					world.DropItem(Coordinates, item);
				}
			}
		}
	}
}