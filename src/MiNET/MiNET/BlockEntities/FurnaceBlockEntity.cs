using System;
using fNbt;
using MiNET.Blocks;
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
		public double BurnTime { get; set; }
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
				if (((MetadataSlot) Inventory.Slots[1]).Value.Id != 0)
				{
					LitFurnace litFurnace = new LitFurnace
					{
						Coordinates = furnace.Coordinates,
						Metadata = furnace.Metadata
					};

					level.SetBlock(litFurnace);
					furnace = litFurnace;

					BurnTime = GetBurnTime();
					CookTime = 0;
					Inventory.DecreasteSlot(1, 1);
				}
			}

			if (furnace is LitFurnace)
			{
				if (BurnTime > 0)
				{
					BurnTime--;
					BurnTick = (short) Math.Ceiling(BurnTime/GetBurnTime()*200d);

					if (((MetadataSlot) Inventory.Slots[0]).Value.Id != 0)
					{
						CookTime++;
						if (CookTime >= 200)
						{
							Inventory.SetSlot(2, new ItemStack(20, 1));
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
					if (!Inventory.DecreasteSlot(1, 1))
					{
						//CookTime = 0;
						BurnTime = GetBurnTime();
						BurnTick = (short) Math.Ceiling(BurnTime/GetBurnTime()*200d);
					}
					else
					{
						Furnace unlitFurnace = new Furnace
						{
							Coordinates = furnace.Coordinates,
							Metadata = furnace.Metadata
						};

						level.SetBlock(unlitFurnace);
						BurnTick = 0;
						BurnTime = 0;
						CookTime = 0;
					}
				}

				level.RelayBroadcast(new McpeContainerSetData
				{
					windowId = 10,
					property = 0,
					value = CookTime
				});

				// Burntime > 0 change block to burning furnace
				level.RelayBroadcast(new McpeContainerSetData
				{
					windowId = 10,
					property = 1,
					value = BurnTick
				});
			}
		}

		private static int GetBurnTime()
		{
			return 300;
		}
	}
}