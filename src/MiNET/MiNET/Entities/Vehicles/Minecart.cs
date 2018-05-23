using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
	public class Minecart : Vehicle
	{
        public int BlockId { get; set; } = 0;
        public int BlockOffset { get; set; } = 6;
        public bool BlockVisible { get; set; } = false;

		public Minecart(Level level, PlayerLocation position) : base(EntityType.Minecart, level)
		{
			KnownPosition = position;
		}

        protected Minecart(EntityType type, Level level, PlayerLocation position) : base(type, level)
        {
            KnownPosition = position;
        }

        public override MetadataDictionary GetMetadata()
        {
            var metadata = base.GetMetadata();
            metadata[(int)MetadataFlags.BlockId] = new MetadataInt(BlockId);
            metadata[(int)MetadataFlags.BlockOffset] = new MetadataInt(BlockOffset);
            metadata[(int)MetadataFlags.BlockVisible] = new MetadataByte(BlockVisible);
            return metadata;
        }

        public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(328)
			};
		}
	}
}
