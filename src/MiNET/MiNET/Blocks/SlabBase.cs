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
using System.Numerics;
using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class SlabBase : Block
	{
		public static Dictionary<string, string> DoubleSlabToSlabMap { get; } = new Dictionary<string, string>();
		public static Dictionary<string, string> SlabToDoubleSlabMap { get; } = new Dictionary<string, string>();

		static SlabBase()
		{
			foreach (var id in BlockFactory.Ids)
			{
				if (id.Contains("_slab") && id.Contains("double_"))
				{
					var slabId = id.Replace("double_", "");
					SlabToDoubleSlabMap.Add(slabId, id);
					DoubleSlabToSlabMap.Add(id, slabId);
				}
			}
		}

		public virtual bool TopSlotBit { get; set; } = false;

		public override BoundingBox GetBoundingBox()
		{
			var bottom = (Vector3)Coordinates;

			if (TopSlotBit) bottom.Y += 0.5f;
			
			var top = bottom + new Vector3(1f, 0.5f, 1f);
			
			return new BoundingBox(bottom, top);
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return base.CanPlace(world, player, blockCoordinates, targetCoordinates, face) || world.GetBlock(blockCoordinates).Id == Id;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var targetBlock = world.GetBlock(targetCoordinates);

			if (targetBlock != null && face == BlockFace.Up && faceCoords.Y == 0.5 && AreSameType(targetBlock))
			{
				// Replace with double block
				SetDoubleSlab(world, targetCoordinates);
				return true;
			}

			if (targetBlock != null && face == BlockFace.Down && faceCoords.Y == 0.5 && AreSameType(targetBlock))
			{
				// Replace with double block
				SetDoubleSlab(world, targetCoordinates);
				return true;
			}

			var existingBlock = world.GetBlock(Coordinates);
			if (existingBlock == null || !AreSameType(existingBlock))
			{
				if (face != BlockFace.Up && faceCoords.Y > 0.5 || (face == BlockFace.Down && faceCoords.Y == 0.0))
				{
					TopSlotBit = true;
				}

				return false;
			}

			// Same material in existing block, make double slab
			// Create double slab, replace existing
			SetDoubleSlab(world, Coordinates);

			return true;
		}

		public override Item GetItem(Level world, bool blockItem = false)
		{
			var item = base.GetItem(world, blockItem) as ItemBlock;
			var block = item.Block as SlabBase;

			block.TopSlotBit = false;

			return item;
		}

		protected virtual bool AreSameType(Block obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != this.GetType()) return false;
			return true;
		}

		protected void SetDoubleSlab(Level world, BlockCoordinates coordinates)
		{
			var slab = BlockFactory.GetBlockById(SlabToDoubleSlabMap[Id]);
			slab.Coordinates = coordinates;
			slab.SetState(GetState());
			world.SetBlock(slab);
		}
	}
}