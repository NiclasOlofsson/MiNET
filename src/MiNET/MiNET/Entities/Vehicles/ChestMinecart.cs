using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
    public class ChestMinecart : Minecart
    {
        public int InventoryId { get; set; }

        public ChestMinecart(Level level, PlayerLocation position) : base(EntityType.ChestMinecart, level, position)
        {
            HealthManager.MaxHealth = 20;
            HealthManager.Health = 20;
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
            metadata[16] = new MetadataInt(0);//inside block id
            metadata[17] = new MetadataInt(6);//inside block offset
            metadata[18] = new MetadataByte(0);//toggle show block inside minecart 
            metadata[22] = new MetadataByte(0);
            metadata[37] = new MetadataLong(0L);
            metadata[38] = new MetadataFloat(1f);
            metadata[42] = new MetadataShort(300);
            metadata[43] = new MetadataInt(0);
            metadata[44] = new MetadataByte(10);//container type
            metadata[45] = new MetadataInt(27);//slots limit
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
            if (actionId == 0)
                player.OpenInventory(EntityId);
            else if(actionId == 1)
            {
                if (player.GameMode == GameMode.Creative)
                    HealthManager.Health = 0;
                else
                    HealthManager.Health -= player.Inventory.GetItemInHand().GetDamage();

                if (HealthManager.Health <= 0)
                {
                    Kill();
                    return;
                }
            }
        }

        public void Kill()
        {
            var inventory = Level.InventoryManager.GetInventoryByEntityId(EntityId);
            Level.InventoryManager.RemoveInventory(inventory.Id);
            Level.DropInventory(KnownPosition + new PlayerLocation(-0.5, 0, -0.5), inventory);
            inventory.CloseInventory();
            DespawnEntity();
        }

        public override Item[] GetDrops()
        {
            return new Item[2]
            {
                ItemFactory.GetItem(328, 0, 1),
                ItemFactory.GetItem(54, 0, 1)
            };
        }
    }
}