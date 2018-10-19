using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class FallingBlock : Entity
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (FallingBlock));


		private readonly Block _original;
		private bool _checkPosition = true;

		public FallingBlock(Level level, Block original) : base((int) EntityType.FallingBlock, level)
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
			metadata[(int) Entity.MetadataFlags.Variant] = new MetadataInt((int)_original.GetRuntimeId());
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
				McpeUpdateBlockSynced updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.coordinates = _original.Coordinates;
				updateBlock.blockRuntimeId = 0;
				updateBlock.blockPriority = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.unknown0 = EntityId;
				updateBlock.unknown1 = 1;

				var addEntity = McpeAddEntity.CreateObject();
				addEntity.entityType = (byte)EntityTypeId;
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
				McpeUpdateBlockSynced updateBlock = McpeUpdateBlockSynced.CreateObject();
				updateBlock.coordinates = new BlockCoordinates(KnownPosition);
				updateBlock.blockRuntimeId = _original.GetRuntimeId();
				updateBlock.blockPriority = 3;
				updateBlock.dataLayerId = 0;
				updateBlock.unknown0 = EntityId;
				updateBlock.unknown1 = 2;

				Level.RelayBroadcast(updateBlock);

				DespawnEntity();

				var block = BlockFactory.GetBlockById(_original.Id);
				block.Metadata = _original.Metadata;
				block.Coordinates = new BlockCoordinates(KnownPosition);
				Level.SetBlock(block, false);
			}
		}

		private void PositionCheck()
		{
			if (Velocity.Y < -0.001)
			{
				int distance = (int) Math.Ceiling(Velocity.Length());
				BlockCoordinates check = new BlockCoordinates(KnownPosition);
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