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
using fNbt;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class StandingBanner : Block
	{
		public int Base { get; set; }
		public NbtCompound ExtraData { get; set; }

		public StandingBanner() : base(176)
		{
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplaceable;
		}

		public override BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, Coordinates + new BlockCoordinates(1, 2, 1));
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// metadata for banner is a value 0-15 that signify the orientation of the banner. Same as PC metadata.
			GroundSignDirection = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180) * 16 / 360) + 0.5) & 0x0f);

			var bannerBlockEntity = new BannerBlockEntity
			{
				Coordinates = Coordinates,
				Base = Base,
			};
			bannerBlockEntity.SetCompound(ExtraData);
			world.SetBlockEntity(bannerBlockEntity);

			return false;
		}
	}
}