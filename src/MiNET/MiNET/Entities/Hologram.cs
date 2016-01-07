using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
    public class Hologram : Entity
    {

        public Hologram(Level level, string text, Vector3 position) : base(64, level)
        {
            NameTag = text;
            KnownPosition = new PlayerLocation(position);
        }

        public override MetadataDictionary GetMetadata()
        {
            var metadata = base.GetMetadata();
            metadata[15] = new MetadataByte(1);
            return metadata;
        }

        public override void OnTick()
        {

        }


    }
}
