using System;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class PrimedTnt : Entity
	{
		public byte Fuse { get; set; }
		public bool Fire { get; set; }
		private bool CheckPosition = true;

		public PrimedTnt(Level level) : base(65, level)
		{
			IsIgnited = true;
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary
			{
				[0] = new MetadataLong(GetDataValue()),
				[56] = new MetadataInt(Fuse)
			};

			return metadata;
		}

		public override void SpawnEntity()
		{
			KnownPosition.X += 0.5f;
			KnownPosition.Y += 0.5f;
			KnownPosition.Z += 0.5f;
			Fire = false;

			base.SpawnEntity();
		}

		public override void OnTick()
		{
			Fuse--;

			if (Fuse == 0)
			{
				DespawnEntity();
				Explode();
			}
			else
			{
				var entityData = McpeSetEntityData.CreateObject();
				entityData.runtimeEntityId = EntityId;
				entityData.metadata = GetMetadata();
				Level.RelayBroadcast(entityData);
				if (CheckPosition) PositionCheck();
			}
		}

		private void PositionCheck()
		{
			BlockCoordinates check = KnownPosition.GetCoordinates3D() + Level.Down;
			if (!Level.GetBlock(check).IsSolid)
			{
				KnownPosition.Y -= 1;
			}
			else
			{
				CheckPosition = false;
			}
		}

		private void Explode()
		{
			// Litteral "fire and forget"
			new Explosion(Level,
				new BlockCoordinates((int) Math.Floor(KnownPosition.X), (int) Math.Floor(KnownPosition.Y), (int) Math.Floor(KnownPosition.Z)), 4, Fire)
				.Explode();
		}
	}
}