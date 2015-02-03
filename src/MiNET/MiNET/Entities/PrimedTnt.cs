using System;
using System.Threading.Tasks;
using Craft.Net.Common;
using MiNET.Net;
using MiNET.Worlds;
using MetadataByte = MiNET.Utils.MetadataByte;
using MetadataDictionary = MiNET.Utils.MetadataDictionary;

namespace MiNET.Entities
{
	public class PrimedTnt : Entity
	{
		public byte Fuse { get; set; }
		private bool CheckPosition = true;

		public PrimedTnt(Level level) : base(65, level)
		{
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[16] = new MetadataByte(Fuse);

			return metadata;
		}

		public override void SpawnEntity()
		{
			KnownPosition.X += 0.5f;
			KnownPosition.Y += 0.5f;
			KnownPosition.Z += 0.5f;

			base.SpawnEntity();
		}

		protected override void OnTick()
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
				entityData.entityId = EntityId;
				entityData.namedtag = GetMetadata().GetBytes();
				Level.RelayBroadcast(entityData, false);
				if (CheckPosition) PositionCheck();
			}
		}

		private void PositionCheck()
		{
			Coordinates3D check = KnownPosition.GetCoordinates3D() + Worlds.Level.Down;
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
				new Coordinates3D((int) Math.Floor(KnownPosition.X), (int) Math.Floor(KnownPosition.Y), (int) Math.Floor(KnownPosition.Z)), 4)
				.Explode();
		}
	}
}