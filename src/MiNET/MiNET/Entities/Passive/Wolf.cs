using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Wolf : PassiveMob
	{
		public bool IsAngry { get; set; }
		public byte CollarColor { get; set; }

		public Wolf(Level level) : base(EntityType.Wolf, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
			IsAngry = false;
			CollarColor = 14;
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[18] = new MetadataInt(IsAngry ? 1 : 0);
			metadata[20] = new MetadataByte(CollarColor);

			return metadata;
		}
	}
}