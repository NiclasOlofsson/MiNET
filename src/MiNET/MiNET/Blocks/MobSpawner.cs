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

using System;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class MobSpawner : Block
	{
		public MobSpawner() : base(52)
		{
			IsTransparent = true; // Doesn't block light
			LightLevel = 1;
			BlastResistance = 25;
			Hardness = 5;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}

		public override float GetExperiencePoints()
		{
			Random random = new Random();
			return random.Next(15, 44);
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			MobSpawnerBlockEntity blockEntity = new MobSpawnerBlockEntity()
			{
				Coordinates = Coordinates
			};
			world.SetBlockEntity(blockEntity);

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			if (player.Inventory.GetItemInHand() is ItemSpawnEgg monsterEgg)
			{
				if (world.GetBlockEntity(Coordinates) is MobSpawnerBlockEntity blockEntity)
				{
					Entity entity = monsterEgg.Metadata.CreateEntity(world);
					if (entity != null)
					{
						blockEntity.EntityTypeId = (int)EntityHelpers.ToEntityType(entity.EntityTypeId);
						blockEntity.DisplayEntityHeight = (float) entity.Height;
						blockEntity.DisplayEntityWidth = (float) entity.Width;
						blockEntity.DisplayEntityScale = (float) entity.Scale;

						world.SetBlockEntity(blockEntity);
					}
				}
			}

			return true;
		}
	}
}