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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBanner : ItemBlock
	{
		public ItemBanner() : base(446, 0)
		{
			MaxStackSize = 16;
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (face == BlockFace.Up) // On top of block
			{
				var banner = new StandingBanner();
				banner.Coordinates = coor;
				// metadata for banner is a value 0-15 that signify the orientation of the banner. Same as PC metadata.
				banner.Metadata = (byte) ((int) (Math.Floor((player.KnownPosition.Yaw + 180)*16/360) + 0.5) & 0x0f);
				world.SetBlock(banner);
			}
			else if (face == BlockFace.North) // At the bottom of block
			{
				// Doesn't work, ignore if that happen. 
				return;
			}
			else
			{
				var banner = new WallBanner();
				banner.Coordinates = coor;
				banner.Metadata = (byte) face;
				world.SetBlock(banner);
			}

			// Then we create and set the wall block entity that has all the intersting data

			var bannerBlockEntity = new BannerBlockEntity
			{
				Coordinates = coor,
				Base = Metadata,
			};
			bannerBlockEntity.SetCompound(ExtraData);

			world.SetBlockEntity(bannerBlockEntity);
		}
	}
}