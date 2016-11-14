using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fNbt;
using MiNET.Items;

namespace MiNET.BlockEntities
{
    public class EnderChestBlockEntity : BlockEntity
    {
        private NbtCompound Compound { get; set; }

        public EnderChestBlockEntity() : base("EnderChest")
        {
            Compound = new NbtCompound(string.Empty)
            {
                new NbtString("id", Id),
                new NbtList("Items", new NbtCompound()),
                new NbtInt("x", Coordinates.X),
                new NbtInt("y", Coordinates.Y),
                new NbtInt("z", Coordinates.Z)
            };

            NbtList items = (NbtList)Compound["Items"];
            for (byte i = 0; i < 27; i++)
            {
                items.Add(new NbtCompound
                {
                    new NbtByte("Slot", i),
                    new NbtShort("id", 0),
                    new NbtShort("Damage", 0),
                    new NbtByte("Count", 0),
                });
            }
        }

        public override NbtCompound GetCompound()
        {
            Compound["x"] = new NbtInt("x", Coordinates.X);
            Compound["y"] = new NbtInt("y", Coordinates.Y);
            Compound["z"] = new NbtInt("z", Coordinates.Z);

            return Compound;
        }

        public override void SetCompound(NbtCompound compound)
        {
            Compound = compound;

            if (Compound["Items"] == null)
            {
                NbtList items = new NbtList("Items");
                for (byte i = 0; i < 27; i++)
                {
                    items.Add(new NbtCompound()
                    {
                        new NbtByte("Slot", i),
                        new NbtShort("id", 0),
                        new NbtShort("Damage", 0),
                        new NbtByte("Count", 0),
                    });
                }
                Compound["Items"] = items;
            }
        }

        public override List<Item> GetDrops()
        {
            return new List<Item>();
        }
    }
}
