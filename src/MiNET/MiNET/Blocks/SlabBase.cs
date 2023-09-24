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
using System.Runtime.CompilerServices;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class SlabBase : Block
	{
		[StateBit] public virtual bool TopSlotBit { get; set; } = false;

		protected SlabBase(string id = null) : base(id)
		{
		}

		public override BoundingBox GetBoundingBox()
		{
			var bottom = (Vector3)Coordinates;

			if (TopSlotBit)
				bottom.Y += 0.5f;
			
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

		protected virtual bool AreSameType(Block obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != this.GetType()) return false;
			return true;
		}

		protected void SetDoubleSlab(Level world, BlockCoordinates coordinates)
		{
			var id = Id.Replace("minecraft:", "");
			id = id.Contains("stone_block_slab") ? $"double_{id}" : id.Replace("slab", "double_slab");

			Block slab = BlockFactory.GetBlockById(Id);
			slab.Coordinates = coordinates;
			slab.SetState(GetState());
			world.SetBlock(slab);
		}
	}

	public partial class CrimsonSlab : SlabBase { public CrimsonSlab() : base() { } }
	public partial class WarpedSlab : SlabBase { public WarpedSlab() : base() { } }
	public partial class BlackstoneSlab : SlabBase { public BlackstoneSlab() : base() { } }
	public partial class PolishedBlackstoneBrickSlab : SlabBase { public PolishedBlackstoneBrickSlab() : base() { } }
	public partial class PolishedBlackstoneSlab : SlabBase { public PolishedBlackstoneSlab() : base() { } }
	public partial class CrimsonDoubleSlab : Block { public CrimsonDoubleSlab() : base() { } }
	public partial class WarpedDoubleSlab : Block { public WarpedDoubleSlab() : base() { } }
	public partial class BlackstoneDoubleSlab : Block { public BlackstoneDoubleSlab() : base() { } }
	public partial class PolishedBlackstoneBrickDoubleSlab : Block { public PolishedBlackstoneBrickDoubleSlab() : base() { } }
	public partial class PolishedBlackstoneDoubleSlab : Block { public PolishedBlackstoneDoubleSlab() : base() { } }

}