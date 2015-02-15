using System;
using fNbt;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BlockEntities
{
	public class FurnaceBlockEntity : BlockEntity
	{
		private NbtCompound Compound { get; set; }
		public Inventory Inventory { get; set; }

		public short CookTime { get; set; }
		public short BurnTime { get; set; }
		public short BurnTick { get; set; }


		public FurnaceBlockEntity() : base("Furnace")
		{
			UpdatesOnTick = true;

			Compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtList("Items", new NbtCompound()),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			NbtList items = (NbtList) Compound["Items"];
			for (byte i = 0; i < 3; i++)
			{
				items.Add(new NbtCompound("")
				{
					new NbtByte("Count", 0),
					new NbtByte("Slot", i),
					new NbtShort("id", 0),
					new NbtByte("Damage", 0),
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
				for (byte i = 0; i < 3; i++)
				{
					items.Add(new NbtCompound("")
					{
						new NbtByte("Count", 0),
						new NbtByte("Slot", i),
						new NbtShort("id", 0),
						new NbtByte("Damage", 0),
					});
				}
				Compound["Items"] = items;
			}
		}

		public override void OnTick(Level level)
		{
			if (Inventory == null) return;

			Furnace furnace = level.GetBlock(Coordinates) as Furnace;
			if (furnace == null) return;

			if (!(furnace is LitFurnace))
			{
				if (GetFuel().Id != 0)
				{
					LitFurnace litFurnace = new LitFurnace
					{
						Coordinates = furnace.Coordinates,
						Metadata = furnace.Metadata
					};

					level.SetBlock(litFurnace);
					furnace = litFurnace;

					BurnTime = GetFuelEfficiency(GetFuel());
					FuelEfficiency = BurnTime;
					CookTime = 0;
					Inventory.DecreasteSlot(1);
				}
			}

			if (!(furnace is LitFurnace)) return;

			if (BurnTime > 0)
			{
				BurnTime--;
				BurnTick = (short) Math.Ceiling((double) BurnTime/FuelEfficiency*200d);

				if (GetIngredient().Id != 0)
				{
					CookTime++;
					if (CookTime >= 200)
					{
						Inventory.IncreasteSlot(2, 20);
						CookTime = 0;
					}
				}
				else
				{
					CookTime = 0;
				}
			}

			if (BurnTime <= 0)
			{
				if (!Inventory.DecreasteSlot(1))
				{
					//CookTime = 0;
					BurnTime = GetFuelEfficiency(GetFuel());
					FuelEfficiency = BurnTime;
					BurnTick = (short) Math.Ceiling((double) BurnTime/FuelEfficiency*200d);
				}
				else
				{
					// No more fule
					Furnace unlitFurnace = new Furnace
					{
						Coordinates = furnace.Coordinates,
						Metadata = furnace.Metadata
					};

					level.SetBlock(unlitFurnace);
					FuelEfficiency = 0;
					BurnTick = 0;
					BurnTime = 0;
					CookTime = 0;
				}
			}

			level.RelayBroadcast(new McpeContainerSetData
			{
				windowId = Inventory.Id,
				property = 0,
				value = CookTime
			});

			level.RelayBroadcast(new McpeContainerSetData
			{
				windowId = Inventory.Id,
				property = 1,
				value = BurnTick
			});
		}

		public short FuelEfficiency { get; set; }

		private ItemStack GetFuel()
		{
			return ((MetadataSlot) Inventory.Slots[1]).Value;
		}

		private ItemStack GetIngredient()
		{
			return ((MetadataSlot) Inventory.Slots[0]).Value;
		}

		private short GetFuelEfficiency(ItemStack itemStack)
		{
			Item item = ItemFactory.GetItem(itemStack.Id, itemStack.Metadata);
			return (short) (item.GetFuelEfficiency()*20);
		}
	}
}