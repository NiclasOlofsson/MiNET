using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public enum CatType
	{
		WildOcelot,
		Tuxedo,
		Tabby,
		Siamese,
	}

	public class Ocelot : PassiveMob
	{
		public CatType CatType { get; set; }

		public Ocelot(Level level) : base(EntityType.Ocelot, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
			CatType = CatType.WildOcelot;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[18] = new MetadataByte((byte) CatType);

			return metadata;
		}
	}
}