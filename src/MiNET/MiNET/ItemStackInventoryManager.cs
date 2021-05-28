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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET
{
	public class ItemStackInventoryManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemStackInventoryManager));

		private readonly Player _player;

		public ItemStackInventoryManager(Player player)
		{
			_player = player;
		}

		public virtual List<StackResponseContainerInfo> HandleItemStackActions(int requestId, ItemStackActionList actions)
		{
			var stackResponses = new List<StackResponseContainerInfo>();
			uint recipeNetworkId = 0;
			foreach (ItemStackAction stackAction in actions)
			{
				switch (stackAction)
				{
					case CraftAction craftAction:
					{
						recipeNetworkId = ProcessCraftAction(craftAction);
						break;
					}
					case CraftCreativeAction craftCreativeAction:
					{
						ProcessCraftCreativeAction(craftCreativeAction);
						break;
					}
					case CraftNotImplementedDeprecatedAction craftNotImplementedDeprecatedAction:
					{
						// Do nothing democrafts
						ProcessCraftNotImplementedDeprecatedAction(craftNotImplementedDeprecatedAction);
						break;
					}
					case CraftRecipeOptionalAction craftRecipeOptionalAction:
					{
						ProcessCraftRecipeOptionalAction(craftRecipeOptionalAction);
						break;
					}
					case CraftResultDeprecatedAction craftResultDeprecatedAction:
					{
						ProcessCraftResultDeprecatedAction(craftResultDeprecatedAction);
						break;
					}
					case TakeAction takeAction:
					{
						ProcessTakeAction(takeAction, stackResponses);

						break;
					}
					case PlaceAction placeAction:
					{
						ProcessPlaceAction(placeAction, stackResponses);
						break;
					}
					case SwapAction swapAction:
					{
						ProcessSwapAction(swapAction, stackResponses);
						break;
					}
					case DestroyAction destroyAction:
					{
						ProcessDestroyAction(destroyAction, stackResponses);
						break;
					}
					case DropAction dropAction:
					{
						ProcessDropAction(dropAction, stackResponses);

						break;
					}
					case ConsumeAction consumeAction:
					{
						ProcessConsumeAction(consumeAction, stackResponses);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException(nameof(stackAction));
				}
			}

			foreach (IGrouping<byte, StackResponseContainerInfo> stackResponseGroup in stackResponses.GroupBy(r => r.ContainerId))
			{
				if (stackResponseGroup.Count() > 1)
				{
					byte containerId = stackResponseGroup.Key;
					StackResponseSlotInfo slotToKeep = null;
					foreach (IGrouping<byte, StackResponseSlotInfo> slotGroup in stackResponseGroup.SelectMany(d => d.Slots).GroupBy(s => s.Slot))
					{
						byte slot = slotGroup.Key;
						if (slotGroup.Count() > 1)
						{
							slotToKeep = slotGroup.ToList().Last();
						}
					}
					if (slotToKeep != null)
					{
						foreach (StackResponseContainerInfo containerInfo in stackResponseGroup)
						{
							if (!containerInfo.Slots.Contains(slotToKeep))
							{
								stackResponses.Remove(containerInfo);
							}
						}
					}
				}
			}

			return stackResponses;
		}

		protected virtual void ProcessConsumeAction(ConsumeAction action, List<StackResponseContainerInfo> stackResponses)
		{
			StackRequestSlotInfo source = action.Source;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			ProcessSourceItem(source, action.Count, ref sourceItem);

			stackResponses.Add(BuildBaseContainerInfo(source, sourceItem));
		}

		protected virtual void ProcessDropAction(DropAction action, List<StackResponseContainerInfo> stackResponses)
		{
			StackRequestSlotInfo source = action.Source;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			Item dropItem = ProcessSourceItem(source, action.Count, ref sourceItem);
			_player.DropItem(dropItem);

			stackResponses.Add(BuildBaseContainerInfo(source, sourceItem));
		}

		protected virtual void ProcessDestroyAction(DestroyAction action, List<StackResponseContainerInfo> stackResponses)
		{
			StackRequestSlotInfo source = action.Source;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			ProcessSourceItem(source, action.Count, ref sourceItem);

			stackResponses.Add(BuildBaseContainerInfo(source, sourceItem));
		}

		protected virtual void ProcessSwapAction(SwapAction action, List<StackResponseContainerInfo> stackResponses)
		{
			StackRequestSlotInfo source = action.Source;
			StackRequestSlotInfo destination = action.Destination;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			Item destItem = GetContainerItem(destination.ContainerId, destination.Slot);

			SetContainerItem(source.ContainerId, source.Slot, destItem);
			SetContainerItem(destination.ContainerId, destination.Slot, sourceItem);

			if (source.ContainerId == (int) ContainerType.Enchanting21
				|| source.ContainerId == (int) ContainerType.Enchanting22
				|| destination.ContainerId == (int) ContainerType.Enchanting21
				|| destination.ContainerId == (int) ContainerType.Enchanting22)
			{
				if (GetContainerItem((int) ContainerType.Enchanting21, 14) is not ItemAir
					&& GetContainerItem((int) ContainerType.Enchanting22, 15) is not ItemAir)
				{
					Item item = GetContainerItem((int) ContainerType.Enchanting21, 14);
					Enchantment.SendEnchantments(_player, item);
				}
				else
				{
					Enchantment.SendEmptyEnchantments(_player);
				}
			}

			stackResponses.Add(BuildBaseContainerInfo(source, destItem));
			stackResponses.Add(BuildBaseContainerInfo(destination, sourceItem));
		}

		protected virtual void ProcessPlaceAction(PlaceAction action, List<StackResponseContainerInfo> stackResponses)
		{
			byte count = action.Count;
			StackRequestSlotInfo source = action.Source;
			StackRequestSlotInfo destination = action.Destination;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			Item destItem = ProcessSourceItem(source, action.Count, ref sourceItem);
			Item existingItem = GetContainerItem(destination.ContainerId, destination.Slot);

			if (existingItem.UniqueId > 0) // is empty/air is what this means
			{
				existingItem.Count += count;
				destItem = existingItem;
			}
			else
			{
				SetContainerItem(destination.ContainerId, destination.Slot, destItem);
			}

			if (destination.ContainerId == (int) ContainerType.Enchanting21 
				|| destination.ContainerId == (int) ContainerType.Enchanting22)
			{
				if (GetContainerItem((int) ContainerType.Enchanting21, 14) is not ItemAir 
					&& GetContainerItem((int) ContainerType.Enchanting22, 15) is not ItemAir)
				{
					Item item = GetContainerItem((int) ContainerType.Enchanting21, 14);
					Enchantment.SendEnchantments(_player, item);
				}
				else
				{
					Enchantment.SendEmptyEnchantments(_player);
				}
			}

			stackResponses.Add(BuildBaseContainerInfo(source, sourceItem));
			stackResponses.Add(BuildBaseContainerInfo(destination, destItem));
		}

		protected virtual void ProcessTakeAction(TakeAction action, List<StackResponseContainerInfo> stackResponses)
		{
			StackRequestSlotInfo source = action.Source;
			StackRequestSlotInfo destination = action.Destination;

			Item sourceItem = GetContainerItem(source.ContainerId, source.Slot);
			Log.Debug($"Take {sourceItem}");
			Item destItem = ProcessSourceItem(source, action.Count, ref sourceItem);

			SetContainerItem(destination.ContainerId, destination.Slot, destItem);

			if (source.ContainerId == (int) ContainerType.Enchanting21
				|| source.ContainerId == (int) ContainerType.Enchanting22)
			{
				if (GetContainerItem((int) ContainerType.Enchanting21, 14) is not ItemAir 
					&& GetContainerItem((int) ContainerType.Enchanting22, 15) is not ItemAir)
				{
					Item item = GetContainerItem((int) ContainerType.Enchanting21, 14);
					Enchantment.SendEnchantments(_player, item);
				}
				else
				{
					Enchantment.SendEmptyEnchantments(_player);
				}
			}

			stackResponses.Add(BuildBaseContainerInfo(source, sourceItem));
			stackResponses.Add(BuildBaseContainerInfo(destination, destItem));
		}

		protected virtual void ProcessCraftResultDeprecatedAction(CraftResultDeprecatedAction action)
		{
			//BUG: Won't work proper with anvil anymore.
			if (GetContainerItem((int) ContainerType.Creative, 50).UniqueId > 0) return;

			//TODO: We only use this for anvils right now. Until we fixed the anvil merge ourselves.
			Item craftingResult = action.ResultItems.FirstOrDefault();
			if (craftingResult == null) return;

			craftingResult.UniqueId = Environment.TickCount;
			SetContainerItem((int) ContainerType.Creative, 50, craftingResult);
		}

		protected virtual void ProcessCraftNotImplementedDeprecatedAction(CraftNotImplementedDeprecatedAction action)
		{
		}

		protected virtual uint ProcessCraftAction(CraftAction action)
		{
			return action.RecipeNetworkId;
		}

		protected virtual void ProcessCraftCreativeAction(CraftCreativeAction action)
		{
			Item creativeItem = InventoryUtils.CreativeInventoryItems.FirstOrDefault(i => i.UniqueId == (int) action.CreativeItemNetworkId);
			if (creativeItem == null) throw new Exception($"Failed to find inventory item with unique id: {action.CreativeItemNetworkId}");
			creativeItem = ItemFactory.GetItem(creativeItem.Id, creativeItem.Metadata);
			creativeItem.Count = (byte) creativeItem.MaxStackSize;
			creativeItem.UniqueId = Environment.TickCount;
			Log.Debug($"Creating {creativeItem}");
			SetContainerItem((int) ContainerType.Creative, 50, creativeItem);
		}

		protected virtual void ProcessCraftRecipeOptionalAction(CraftRecipeOptionalAction action)
		{
		}

		private Item GetContainerItem(int containerId, int slot)
		{
			if (_player.UsingAnvil && containerId < 3) containerId = 13;

			Item item = null;
			switch ((ContainerType) containerId)
			{
				case ContainerType.Crafting:
				case ContainerType.Enchanting21:
				case ContainerType.Enchanting22:
				case ContainerType.Loom:
				case ContainerType.Cursor:
				case ContainerType.Creative:
					item = _player.Inventory.UiInventory.Slots[slot];
					break;
				case ContainerType.Auto:
				case ContainerType.Hotbar:
				case ContainerType.PlayerInventory:
					item = _player.Inventory.Slots[slot];
					break;
				case ContainerType.OffHand:
					item = _player.Inventory.OffHand;
					break;
				case ContainerType.Armor:
					item = (ArmorType) slot switch
					{
						ArmorType.Helmet => _player.Inventory.Helmet,
						ArmorType.Chestplate => _player.Inventory.Chest,
						ArmorType.Leggings => _player.Inventory.Leggings,
						ArmorType.Boots => _player.Inventory.Boots,
						_ => null
					};
					break;
				case ContainerType.Chest:
					if (_player._openInventory is Inventory inventory) item = inventory.GetSlot((byte) slot);
					break;
				default:
					Log.Warn($"Unknown containerId: {containerId}");
					break;
			}

			return item;
		}

		private void SetContainerItem(int containerId, int slot, Item item)
		{
			if (_player.UsingAnvil && containerId < 3) containerId = 13;

			switch ((ContainerType) containerId)
			{
				case ContainerType.Crafting:
				case ContainerType.Enchanting21:
				case ContainerType.Enchanting22:
				case ContainerType.Loom:
				case ContainerType.Cursor:
				case ContainerType.Creative:
					_player.Inventory.UpdateUiSlot(slot, item);
					break;
				case ContainerType.Auto:
				case ContainerType.Hotbar:
				case ContainerType.PlayerInventory:
					_player.Inventory.UpdateInventorySlot(slot, item);
					break;
				case ContainerType.OffHand:
					_player.Inventory.UpdateOffHandSlot(item);
					break;
				case ContainerType.Armor:
					_player.Inventory.UpdateArmorSlot((ArmorType) slot, item);
					_player.SendArmorForPlayer();
					break;
				case ContainerType.Chest:
					if (_player._openInventory is Inventory inventory) inventory.SetSlot(_player, (byte) slot, item);
					break;
				default:
					Log.Warn($"Unknown containerId: {containerId}");
					break;
			}
		}

		#region StackResponse helper

		private Item ProcessSourceItem(StackRequestSlotInfo slotInfo, byte count, ref Item sourceItem)
		{
			Item resultItem;

			if (sourceItem.Count <= count)
			{
				resultItem = sourceItem;
				sourceItem = new ItemAir();
				SetContainerItem(slotInfo.ContainerId, slotInfo.Slot, sourceItem);
			}
			else
			{
				resultItem = (Item) sourceItem.Clone();
				sourceItem.Count -= count;
				resultItem.Count = count;
				resultItem.UniqueId = Environment.TickCount;
			}

			return resultItem;
		}

		private static StackResponseContainerInfo BuildBaseContainerInfo(StackRequestSlotInfo slotInfo, Item item)
		{
			return new StackResponseContainerInfo
			{
				ContainerId = slotInfo.ContainerId,
				Slots = new List<StackResponseSlotInfo> { BuildBaseSlotInfo(item, slotInfo.Slot) }
			};
		}

		private static StackResponseSlotInfo BuildBaseSlotInfo(Item item, byte slot)
		{
			return new StackResponseSlotInfo()
			{
				Count = item.Count,
				Slot = slot,
				HotbarSlot = slot,
				StackNetworkId = item.UniqueId
			};
		}

		#endregion
	}
}