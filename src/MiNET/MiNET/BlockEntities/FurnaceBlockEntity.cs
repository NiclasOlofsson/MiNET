#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using fNbt;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
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
				items.Add(new NbtCompound()
				{
					new NbtByte("Count", 0),
					new NbtByte("Slot", i),
					new NbtShort("id", 0),
					new NbtShort("Damage", 0),
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
					items.Add(new NbtCompound()
					{
						new NbtByte("Count", 0),
						new NbtByte("Slot", i),
						new NbtShort("id", 0),
						new NbtShort("Damage", 0),
					});
				}
				Compound["Items"] = items;
			}
		}

		public override void OnTick(Level level)
		{
			if (Inventory == null) return;

			var furnace = level.GetBlock(Coordinates) as FurnaceBase;
			if (furnace == null) return;

			if (!(furnace is LitFurnace))
			{
				Item fuel = GetFuel();
				Item ingredient = GetIngredient();
				Item smelt = ingredient.GetSmelt();
				// To light a furnace you need both fule and proper ingredient.
				if (fuel.Count > 0 && fuel.FuelEfficiency > 0 && smelt != null)
				{
					LitFurnace litFurnace = new LitFurnace
					{
						Coordinates = furnace.Coordinates,
					};
					litFurnace.SetState(furnace.GetState().States);
					level.SetBlock(litFurnace);
					furnace = litFurnace;

					BurnTime = GetFuelEfficiency(fuel);
					FuelEfficiency = BurnTime;
					CookTime = 0;
					Inventory.DecreaseSlot(1);
				}
			}

			if (furnace is LitFurnace)
			{
				if (BurnTime > 0)
				{
					BurnTime--;
					BurnTick = (short) Math.Ceiling((double) BurnTime / FuelEfficiency * 200d);

					Item ingredient = GetIngredient();
					Item smelt = ingredient.GetSmelt();
					if (smelt != null)
					{
						CookTime++;
						if (CookTime >= 200)
						{
							Inventory.DecreaseSlot(0);
							Inventory.IncreaseSlot(2, smelt.Id, smelt.Metadata);

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
					var fuel = GetFuel();
					Item ingredient = GetIngredient();
					Item smelt = ingredient.GetSmelt();
					if (fuel.Count > 0 && fuel.FuelEfficiency > 0 && smelt != null)
					{
						Inventory.DecreaseSlot(1);

						CookTime = 0;
						BurnTime = GetFuelEfficiency(fuel);
						FuelEfficiency = BurnTime;
						BurnTick = (short) Math.Ceiling((double) BurnTime / FuelEfficiency * 200d);
					}
					else
					{
						// No more fule or nothin more to smelt.
						var unlitFurnace = new Furnace
						{
							Coordinates = furnace.Coordinates,
						};
						unlitFurnace.SetState(furnace.GetState().States);
						level.SetBlock(unlitFurnace);
						FuelEfficiency = 0;
						BurnTick = 0;
						BurnTime = 0;
						CookTime = 0;
					}
				}
			}

			foreach (var observer in Inventory.Observers)
			{
				var cookTimeSetData = McpeContainerSetData.CreateObject();
				cookTimeSetData.windowId = Inventory.WindowsId;
				cookTimeSetData.property = 0;
				cookTimeSetData.value = CookTime;
				observer.SendPacket(cookTimeSetData);

				var burnTimeSetData = McpeContainerSetData.CreateObject();
				burnTimeSetData.windowId = Inventory.WindowsId;
				burnTimeSetData.property = 1;
				burnTimeSetData.value = BurnTick;
				observer.SendPacket(burnTimeSetData);
			}
		}

		private Item GetResult(Item ingredient)
		{
			return ingredient.GetSmelt();
		}

		public short FuelEfficiency { get; set; }

		private Item GetFuel()
		{
			return Inventory.Slots[1];
		}

		private Item GetIngredient()
		{
			return Inventory.Slots[0];
		}

		private short GetFuelEfficiency(Item item)
		{
			return (short) (item.FuelEfficiency * 20);
		}

		public override List<Item> GetDrops()
		{
			List<Item> slots = new List<Item>();

			var items = Compound["Items"] as NbtList;
			if (items == null) return slots;

			for (byte i = 0; i < items.Count; i++)
			{
				NbtCompound itemData = (NbtCompound) items[i];
				Item item = ItemFactory.GetItem(itemData["id"].ShortValue, itemData["Damage"].ShortValue, itemData["Count"].ByteValue);
				slots.Add(item);
			}

			return slots;
		}
	}
}