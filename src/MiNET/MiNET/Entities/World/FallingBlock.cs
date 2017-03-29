using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
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
			Velocity = new Vector3(0, (float) -Gravity, 0);
			NoAi = false;

			Gravity = 0.04;
			Drag = 0.02;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();

			//MetadataDictionary metadata = new MetadataDictionary();
			//metadata[0] = new MetadataLong(0); // 0
			//metadata[1] = new MetadataInt(1);
			metadata[2] = new MetadataInt(_original.Id | (_original.Metadata << 8));
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

		public override void OnTick()
		{
			PositionCheck();

			if (KnownPosition.Y > -1 && _checkPosition)
			{
				Velocity -= new Vector3(0, (float) Gravity, 0);
				Velocity *= (float) (1.0f - Drag);
			}
			else
			{
				DespawnEntity();

				var block = BlockFactory.GetBlockById(_original.Id);
				block.Metadata = _original.Metadata;
				block.Coordinates = new BlockCoordinates(KnownPosition);
				Level.SetBlock(block);
			}
		}

		private void PositionCheck()
		{
			if (Velocity.Y < -0.1)
			{
				int distance = (int) Math.Ceiling(Velocity.Length());
				for (int i = 0; i < distance; i++)
				{
					BlockCoordinates check = new BlockCoordinates(KnownPosition) + (BlockCoordinates.Down*i);
					if (Level.GetBlock(check).IsSolid)
					{
						_checkPosition = false;
						KnownPosition = check + BlockCoordinates.Up;
						return;
					}
				}
			}
			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;
		}
	}
}