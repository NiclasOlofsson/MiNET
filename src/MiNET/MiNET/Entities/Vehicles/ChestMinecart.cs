using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
    public class ChestMinecart : Minecart
    {
        public int InventoryId { get; set; }

        public ChestMinecart(Level level, PlayerLocation position) : base(EntityType.ChestMinecart, level, position)
        {
            HealthManager.MaxHealth = 10;
        }

        public override MetadataDictionary GetMetadata()
        {
            MetadataDictionary metadata = base.GetMetadata();
            metadata[1] = new MetadataInt(40);
            metadata[2] = new MetadataInt(0);
            metadata[3] = new MetadataByte(0);
            metadata[4] = new MetadataString("");
            metadata[5] = new MetadataLong(-1L);
            metadata[7] = new MetadataShort(300);
            metadata[11] = new MetadataInt(0);
            metadata[12] = new MetadataInt(1);
            metadata[16] = new MetadataInt(0);
            metadata[17] = new MetadataInt(6);
            metadata[18] = new MetadataByte(0);
            metadata[22] = new MetadataByte(0);
            metadata[37] = new MetadataLong(0L);
            metadata[38] = new MetadataFloat(1f);
            metadata[42] = new MetadataShort(300);
            metadata[43] = new MetadataInt(0);
            metadata[44] = new MetadataByte(10);
            metadata[45] = new MetadataInt(27);
            metadata[46] = new MetadataInt(0);
            metadata[53] = new MetadataFloat(0.98f);
            metadata[54] = new MetadataFloat(0.7f);
            metadata[57] = new MetadataByte(0);
            metadata[58] = new MetadataFloat(0.0f);
            metadata[59] = new MetadataFloat(0.0f);
            metadata[69] = new MetadataByte(0);
            metadata[70] = new MetadataString("");
            metadata[71] = new MetadataString("");
            metadata[72] = new MetadataByte(1);
            metadata[73] = new MetadataByte(0);
            metadata[74] = new MetadataInt(0);
            metadata[75] = new MetadataInt(0);
            metadata[76] = new MetadataInt(0);
            metadata[77] = new MetadataInt(-1);
            metadata[80] = new MetadataByte(byte.MaxValue);
            metadata[81] = new MetadataByte(0);
            return metadata;
        }

        public override void DoInteraction(byte actionId, Player player)
        {
            player.SendMessage(actionId + "");
            if (actionId == 0)
                player.OpenInventory(EntityId);
            else if (actionId == 1)
            {
                if (player.GameMode == GameMode.Creative)
                    DespawnEntity();
                else
                    HealthManager.TakeHit(null, 4);
            }
        }

        public override Item[] GetDrops()
        {
            return new Item[2]
            {
        ItemFactory.GetItem((short) 328, (short) 0, 1),
        ItemFactory.GetItem((short) 54, (short) 0, 1)
            };
        }
    }
}