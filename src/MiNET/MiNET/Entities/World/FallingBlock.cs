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
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class FallingBlock : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(FallingBlock));


		private readonly int _original;
		private bool _checkPosition = true;

		public FallingBlock(Level level, int original) : base(EntityType.FallingBlock, level)
		{
			_original = original;
			//Gravity = 0.04;
			Height = Width = Length = 0.98;
			Velocity = new Vector3(0, -0.0392f, 0);
			NoAi = false;
			HasCollision = true;
			IsAffectedByGravity = true;

			Gravity = 0.04;
			Drag = 0.02;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();

			//MetadataDictionary metadata = new MetadataDictionary();
			//metadata[0] = new MetadataLong(0); // 0
			//metadata[1] = new MetadataInt(1);
			metadata[(int) MetadataFlags.Variant] = new MetadataInt(_original);
			//metadata[4] = new MetadataString("");
			//metadata[5] = new MetadataLong(-1);
			//metadata[7] = new MetadataShort(300);
			//metadata[39] = new MetadataFloat(1f);
			//metadata[44] = new MetadataShort(300);
			//metadata[45] = new MetadataInt(0);
			//metadata[46] = new MetadataByte(0);
			//metadata[47] = new MetadataInt(0);
			//metadata[53] = new MetadataFloat(0.98f);
			//metadata[54] = new MetadataFloat(0.98f);
			//metadata[56] = new MetadataVector3(0, 0, 0);
			//metadata[57] = new MetadataByte(0);
			//metadata[58] = new MetadataFloat(0f);
			//metadata[59] = new MetadataFloat(0f);

			return metadata;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			//McpeUpdateBlockSynced updateBlock = McpeUpdateBlockSynced.CreateObject();
			//updateBlock.coordinates = _original.Coordinates;
			//updateBlock.blockRuntimeId = 0;
			//updateBlock.blockPriority = 3;
			//updateBlock.dataLayerId = 0;
			//updateBlock.unknown0 = EntityId;
			//updateBlock.unknown1 = 1;

			//Level.RelayBroadcast(players, updateBlock);

			//base.SpawnToPlayers(players);

			foreach (var player in players)
			{
				var updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.coordinates = (BlockCoordinates) KnownPosition;
				updateBlock.blockRuntimeId = (uint) new Air().GetRuntimeId();
				updateBlock.blockPriority = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.unknown0 = EntityId;
				updateBlock.unknown1 = 1;

				var addEntity = McpeAddEntity.CreateObject();
				addEntity.entityType = EntityTypeId;
				addEntity.entityIdSelf = EntityId;
				addEntity.runtimeEntityId = EntityId;
				addEntity.x = KnownPosition.X;
				addEntity.y = KnownPosition.Y;
				addEntity.z = KnownPosition.Z;
				addEntity.pitch = KnownPosition.Pitch;
				addEntity.yaw = KnownPosition.Yaw;
				addEntity.headYaw = KnownPosition.HeadYaw;
				addEntity.metadata = GetMetadata();
				addEntity.speedX = Velocity.X;
				addEntity.speedY = Velocity.Y;
				addEntity.speedZ = Velocity.Z;
				addEntity.attributes = GetEntityAttributes();

				player.SendPacket(updateBlock);
				player.SendPacket(addEntity);
			}
		}

		public override void OnTick(Entity[] entities)
		{
			PositionCheck();

			if (KnownPosition.Y > -1 && _checkPosition)
			{
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity *= (float) (1.0f - Drag);
			}
			else
			{
				var updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.coordinates = new BlockCoordinates(KnownPosition);
				updateBlock.blockRuntimeId = (uint) _original;
				updateBlock.blockPriority = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.unknown0 = EntityId;
				updateBlock.unknown1 = 2;

				Level.RelayBroadcast(updateBlock);

				DespawnEntity();

				var blockState = BlockFactory.BlockPalette[_original];
				var block = BlockFactory.GetBlockById(blockState.Id);
				block.SetState(blockState.States);
				block.Coordinates = (BlockCoordinates) KnownPosition;

				Level.SetBlock(block, false);
			}
		}

		//private void PositionCheck()
		//{
		//	if (Velocity.Y < -0.001)
		//	{
		//		int distance = (int) Math.Ceiling(Velocity.Length());
		//		BlockCoordinates check = new BlockCoordinates(KnownPosition);
		//		for (int i = 0; i < distance; i++)
		//		{
		//			if (Level.GetBlock(check).IsSolid)
		//			{
		//				_checkPosition = false;
		//				KnownPosition = check.BlockUp();
		//				return;
		//			}
		//			check = check.BlockDown();
		//		}
		//	}

		//	KnownPosition.X += (float) Velocity.X;
		//	KnownPosition.Y += (float) Velocity.Y;
		//	KnownPosition.Z += (float) Velocity.Z;
		//}

		private void PositionCheck()
		{
			if (Velocity.Y < -0.001)
			{
				int distance = (int) Math.Ceiling(Velocity.Length());
				var check = new BlockCoordinates(KnownPosition);
				for (int i = 0; i < distance; i++)
				{
					if (Level.GetBlock(check).IsSolid)
					{
						_checkPosition = false;
						KnownPosition = check.BlockUp();
						return;
					}
					check = check.BlockDown();
				}
			}

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;
		}
	}
}