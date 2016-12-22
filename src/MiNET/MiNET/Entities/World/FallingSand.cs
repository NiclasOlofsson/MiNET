using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class FallingSand : Entity
	{
		private bool CheckPosition = true;
		public BlockCoordinates Origin { get; set; }

		public FallingSand(Level level) : base((int) EntityType.FallingBlock, level)
		{
			Gravity = 0.04;
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataLong(0); // 0
			metadata[1] = new MetadataInt(1);
			metadata[2] = new MetadataInt(12);
			metadata[3] = new MetadataByte(0);
			metadata[4] = new MetadataString("");
			metadata[5] = new MetadataLong(-1);
			metadata[7] = new MetadataShort(300);
			metadata[39] = new MetadataFloat(1f);
			metadata[44] = new MetadataShort(300);
			metadata[45] = new MetadataInt(0);
			metadata[46] = new MetadataByte(0);
			metadata[47] = new MetadataInt(0);
			metadata[53] = new MetadataFloat(0.98f);
			metadata[54] = new MetadataFloat(0.98f);
			metadata[56] = new MetadataVector3(0, 0, 0);
			metadata[57] = new MetadataByte(0);
			metadata[58] = new MetadataFloat(0f);
			metadata[59] = new MetadataFloat(0f);

			return metadata;
		}

		public override void SpawnEntity()
		{
			KnownPosition.X += 0.4f;
			//KnownPosition.Y += 0.4f;
			KnownPosition.Z += 0.4f;

			base.SpawnEntity();
		}

		public override void OnTick()
		{
			if (KnownPosition.Y > -1 && CheckPosition)
			{
				PositionCheck();

				Velocity *= (float) (1.0f - Drag);
				Velocity -= new Vector3(0, (float) Gravity, 0);
			}
			else
			{
				DespawnEntity();
				Level.SetBlock(new Sand {Coordinates = new BlockCoordinates(KnownPosition)});
			}
		}

		private void PositionCheck()
		{
			BlockCoordinates check = new BlockCoordinates(KnownPosition) + Level.Down;
			if (!Level.GetBlock(check).IsSolid)
			{
				KnownPosition.X += (float)Velocity.X;
				KnownPosition.Y += (float)Velocity.Y;
				KnownPosition.Z += (float)Velocity.Z;
			}
			else
			{
				CheckPosition = false;
			}
		}
	}
}