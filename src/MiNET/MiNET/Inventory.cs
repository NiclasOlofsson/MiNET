using System;
using Craft.Net.Common;
using MiNET.Utils;
using MetadataSlot = MiNET.Utils.MetadataSlot;

namespace MiNET
{
	public class Inventory
	{
		public event Action<Inventory> InventoryChange;

		public byte Id { get; set; }
		public byte Type { get; set; }
		public MetadataSlots Slots { get; set; }
		public short Size { get; set; }
		public Coordinates3D Coordinates { get; set; }


		public void SetSlot(byte slot, MetadataSlot metadataSlot)
		{
			Slots[slot] = metadataSlot;
			OnInventoryChange();
		}

		public void InventoryChanged()
		{
			OnInventoryChange();
		}

		protected virtual void OnInventoryChange()
		{
			Action<Inventory> handler = InventoryChange;
			if (handler != null) handler(this);
		}
	}
}