using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Wolf : PassiveMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Wolf));

		public bool IsAngry { get; set; }
		public byte CollarColor { get; set; }
		public Entity Owner { get; set; }

		public Wolf(Level level) : base(EntityType.Wolf, level)
		{
			NameTag = "Steve The Dog";
			Width = Length = 0.6;
			Height = 0.8;
			IsAngry = true;
			CollarColor = 14;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[1] = new MetadataInt(12);
			metadata[2] = new MetadataInt(0);
			metadata[3] = new MetadataByte(11); // Collar color
			metadata[4] = new MetadataString("Testing");
			metadata[5] = new MetadataLong(Owner.EntityId);
			metadata[7] = new MetadataShort(300);
			metadata[8] = new MetadataInt(0);
			metadata[9] = new MetadataByte(0);
			metadata[38] = new MetadataLong(0);
			metadata[39] = new MetadataFloat(1.0f);
			metadata[44] = new MetadataShort(300);
			metadata[45] = new MetadataInt(0);
			metadata[46] = new MetadataByte(0);
			metadata[47] = new MetadataInt(0);
			metadata[53] = new MetadataFloat(0.6f);
			metadata[54] = new MetadataFloat(0.8f);
			metadata[56] = new MetadataVector3(0, 0, 0);
			metadata[57] = new MetadataByte(0);
			metadata[58] = new MetadataFloat(0f);
			metadata[59] = new MetadataFloat(0f);

			return metadata;
		}
	}
}