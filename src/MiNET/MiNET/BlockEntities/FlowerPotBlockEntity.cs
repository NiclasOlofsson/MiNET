using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fNbt;
using MiNET.Items;

namespace MiNET.BlockEntities
{
    public class FlowerPotBlockEntity : BlockEntity
    {
        private NbtCompound Compound { get; set; }

        public FlowerPotBlockEntity() : base("FlowerPot")
		{
            Compound = new NbtCompound(string.Empty)
            {
                new NbtString("id", Id),
                new NbtShort("item", 0),
                new NbtInt("mData", 0),
                new NbtInt("x", Coordinates.X),
                new NbtInt("y", Coordinates.Y),
                new NbtInt("z", Coordinates.Z),
            };
            
            //Log.Error($"New ItemFrame block entity: {Compound}");
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
        }

        public void SetItem(Item item)
        {
            var comp = new NbtCompound(string.Empty)
            {
                new NbtString("id", Id),
                new NbtShort("item", item.Id),
                new NbtInt("mData", item.Metadata),
                new NbtInt("x", Coordinates.X),
                new NbtInt("y", Coordinates.Y),
                new NbtInt("z", Coordinates.Z),
            };
            
            Compound = comp;
        }

        public override List<Item> GetDrops()
        {
            List<Item> slots = new List<Item>();

            if (Compound["item"].ShortValue == 0) return slots;

            Item item = ItemFactory.GetItem(Compound["item"].ShortValue, Compound["mData"].ShortValue);
            slots.Add(item);

            return slots;
        }
    }
}
