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
using fNbt;
using log4net;
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Net;
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

		public virtual List<ItemStackResponseContainerInfo> HandleItemStackActions(int requestId, ItemStackRequest request)
		{
			var stackResponses = new List<ItemStackResponseContainerInfo>();
			_activeRecipe = null;
			foreach (ItemStackRequestBase stackAction in request.actions ?? new List<ItemStackRequestBase>())
			{
				switch (stackAction)
				{
					case ItemStackRequestCraftRecipeAction craftAction:
					{
						ProcessCraftAction(craftAction);
						break;
					}
					case ItemStackRequestCraftRecipeAutoAction craftAutoAction:
					{
						ProcessCraftAutoAction(craftAutoAction);
						break;
					}
					case ItemStackRequestCraftCreativeAction craftCreativeAction:
					{
						ProcessCraftCreativeAction(craftCreativeAction);
						break;
					}
					case ItemStackRequestCraftNonImplementedDeprecatedAction craftNotImplementedDeprecatedAction:
					{
						// Do nothing democrafts
						ProcessCraftNotImplementedDeprecatedAction(craftNotImplementedDeprecatedAction);
						break;
					}
					case ItemStackRequestCraftRecipeOptionalAction craftRecipeOptionalAction:
					{
						ProcessCraftRecipeOptionalAction(craftRecipeOptionalAction);
						break;
					}
					case ItemStackRequestCraftResultsDeprecatedAction craftResultDeprecatedAction:
					{
						ProcessCraftResultDeprecatedAction(craftResultDeprecatedAction);
						break;
					}
					case ItemStackRequestTakeAction takeAction:
					{
						ProcessTakeAction(takeAction, stackResponses);

						break;
					}
					case ItemStackRequestPlaceAction placeAction:
					{
						ProcessPlaceAction(placeAction, stackResponses);
						break;
					}
					case ItemStackRequestSwapAction swapAction:
					{
						ProcessSwapAction(swapAction, stackResponses);
						break;
					}
					case ItemStackRequestDestroyAction destroyAction:
					{
						ProcessDestroyAction(destroyAction, stackResponses);
						break;
					}
					case ItemStackRequestDropAction dropAction:
					{
						ProcessDropAction(dropAction, stackResponses);

						break;
					}
					case ItemStackRequestConsumeAction consumeAction:
					{
						ProcessConsumeAction(consumeAction, stackResponses);
						break;
					}
					case ItemStackRequestCraftLoomAction craftLoomAction:
					{
						ProcessCraftLoomAction(craftLoomAction);
						break;
					}
					case ItemStackRequestCraftRepairAndDisenchantAction repairAction:
					{
						ProcessCraftRepairAndDisenchantAction(repairAction);
						break;
					}
					case ItemStackRequestBeaconPaymentAction beaconPaymentAction:
					{
						ProcessBeaconPaymentAction(beaconPaymentAction);
						break;
					}
					case ItemStackRequestLabTableCombineAction labTableCombineAction:
					{
						ProcessLabTableCombineAction(labTableCombineAction);
						break;
					}
					case ItemStackRequestCreateAction createAction:
					{
						ProcessCreateAction(createAction);
						break;
					}
					case ItemStackRequestMineBlockAction mineBlockAction:
					{
						ProcessMineBlockAction(mineBlockAction);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException(nameof(stackAction));
				}
			}

			foreach (IGrouping<FullContainerName.ContainerEnumName, ItemStackResponseContainerInfo> stackResponseGroup in stackResponses.GroupBy(r => r.fullContainerName.containerName))
			{
				if (stackResponseGroup.Count() > 1)
				{
					FullContainerName.ContainerEnumName containerName = stackResponseGroup.Key;
					ItemStackResponseSlotInfo slotToKeep = null;
					foreach (IGrouping<byte, ItemStackResponseSlotInfo> slotGroup in stackResponseGroup.SelectMany(d => d.slots).GroupBy(s => s.requestedSlot))
					{
						byte requestedSlot = slotGroup.Key;
						if (slotGroup.Count() > 1)
						{
							slotToKeep = slotGroup.ToList().Last();
						}
					}
					if (slotToKeep != null)
					{
						foreach (ItemStackResponseContainerInfo containerInfo in stackResponseGroup)
						{
							if (!containerInfo.slots.Contains(slotToKeep))
							{
								stackResponses.Remove(containerInfo);
							}
						}
					}
				}
			}

			return stackResponses;
		}

		protected virtual void ProcessConsumeAction(ItemStackRequestConsumeAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			byte count = action.amount;
			ItemStackRequestSlotInfo source = action.source;

			Item sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);
			sourceItem.Count -= count;
			if (sourceItem.Count <= 0)
			{
				sourceItem = new ItemAir();
				_player.SetContainerItem(source.fullContainerName.containerName, source.slot, sourceItem);
			}

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessDropAction(ItemStackRequestDropAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			byte count = action.amount;
			Item dropItem;
			ItemStackRequestSlotInfo source = action.source;

			Item sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);

			if (sourceItem.Count == count || sourceItem.Count - count <= 0)
			{
				dropItem = sourceItem;
				sourceItem = new ItemAir();
				sourceItem.UniqueId = 0;
				_player.SetContainerItem(source.fullContainerName.containerName, source.slot, sourceItem);
			}
			else
			{
				dropItem = (Item) sourceItem.Clone();
				sourceItem.Count -= count;
				dropItem.Count = count;
				dropItem.UniqueId = Environment.TickCount;
			}

			_player.DropItem(dropItem);

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessDestroyAction(ItemStackRequestDestroyAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			byte count = action.amount;
			ItemStackRequestSlotInfo source = action.source;

			Item sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);
			sourceItem.Count -= count;
			if (sourceItem.Count <= 0)
			{
				sourceItem = new ItemAir();
				_player.SetContainerItem(source.fullContainerName.containerName, source.slot, sourceItem);
			}

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessSwapAction(ItemStackRequestSwapAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			ItemStackRequestSlotInfo source = action.source;
			ItemStackRequestSlotInfo destination = action.destination;

			Item sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);
			Item destItem = _player.GetContainerItem(destination.fullContainerName.containerName, destination.slot);

			_player.SetContainerItem(source.fullContainerName.containerName, source.slot, destItem);
			_player.SetContainerItem(destination.fullContainerName.containerName, destination.slot, sourceItem);

			if (source.fullContainerName.containerName == FullContainerName.ContainerEnumName.RecipeBookContainer || source.fullContainerName.containerName == FullContainerName.ContainerEnumName.EnchantingInputContainer || destination.fullContainerName.containerName == FullContainerName.ContainerEnumName.RecipeBookContainer || destination.fullContainerName.containerName == FullContainerName.ContainerEnumName.EnchantingInputContainer)
			{
				if (!(_player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14) is ItemAir) && !(_player.GetContainerItem(FullContainerName.ContainerEnumName.EnchantingInputContainer, 15) is ItemAir)) Enchantment.SendEnchantments(_player, _player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14));
				else Enchantment.SendEmptyEnchantments(_player);
			}

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = destItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = destItem.UniqueId > 0 ? destItem.UniqueId : null
					}
				}
			});
			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) destination.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = destination.slot,
						slot = destination.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessPlaceAction(ItemStackRequestPlaceAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			byte count = action.amount;
			Item sourceItem;
			Item destItem;
			ItemStackRequestSlotInfo source = action.source;
			ItemStackRequestSlotInfo destination = action.destination;

			sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);

			if (sourceItem.Count == count || sourceItem.Count - count <= 0)
			{
				destItem = sourceItem;
				sourceItem = new ItemAir();
				sourceItem.UniqueId = 0;
				_player.SetContainerItem(source.fullContainerName.containerName, source.slot, sourceItem);
			}
			else
			{
				destItem = (Item) sourceItem.Clone();
				sourceItem.Count -= count;
				destItem.Count = count;
				destItem.UniqueId = Environment.TickCount;
			}

			Item existingItem = _player.GetContainerItem(destination.fullContainerName.containerName, destination.slot);
			if (existingItem.UniqueId > 0) // is empty/air is what this means
			{
				existingItem.Count += count;
				destItem = existingItem;
			}
			else
			{
				_player.SetContainerItem(destination.fullContainerName.containerName, destination.slot, destItem);
			}

			if (destination.fullContainerName.containerName == FullContainerName.ContainerEnumName.RecipeBookContainer || destination.fullContainerName.containerName == FullContainerName.ContainerEnumName.EnchantingInputContainer)
			{
				if (!(_player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14) is ItemAir) && !(_player.GetContainerItem(FullContainerName.ContainerEnumName.EnchantingInputContainer, 15) is ItemAir)) Enchantment.SendEnchantments(_player, _player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14));
				else Enchantment.SendEmptyEnchantments(_player);
			}

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) destination.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = destItem.Count,
						requestedSlot = destination.slot,
						slot = destination.slot,
						itemStackNetId = destItem.UniqueId > 0 ? destItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessTakeAction(ItemStackRequestTakeAction action, List<ItemStackResponseContainerInfo> stackResponses)
		{
			byte count = action.amount;
			Item sourceItem;
			Item destItem;
			ItemStackRequestSlotInfo source = action.source;
			ItemStackRequestSlotInfo destination = action.destination;

			sourceItem = _player.GetContainerItem(source.fullContainerName.containerName, source.slot);
			Log.Debug($"Take {sourceItem}");

			if (sourceItem.Count == count || sourceItem.Count - count <= 0)
			{
				destItem = sourceItem;
				sourceItem = new ItemAir();
				sourceItem.UniqueId = 0;
				_player.SetContainerItem(source.fullContainerName.containerName, source.slot, sourceItem);
			}
			else
			{
				destItem = (Item) sourceItem.Clone();
				sourceItem.Count -= count;
				destItem.Count = count;
				destItem.UniqueId = Environment.TickCount;
			}

			_player.SetContainerItem(destination.fullContainerName.containerName, destination.slot, destItem);

			if (source.fullContainerName.containerName == FullContainerName.ContainerEnumName.RecipeBookContainer || source.fullContainerName.containerName == FullContainerName.ContainerEnumName.EnchantingInputContainer)
			{
				if (!(_player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14) is ItemAir) && !(_player.GetContainerItem(FullContainerName.ContainerEnumName.EnchantingInputContainer, 15) is ItemAir)) Enchantment.SendEnchantments(_player, _player.GetContainerItem(FullContainerName.ContainerEnumName.RecipeBookContainer, 14));
				else Enchantment.SendEmptyEnchantments(_player);
			}

			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) source.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = sourceItem.Count,
						requestedSlot = source.slot,
						slot = source.slot,
						itemStackNetId = sourceItem.UniqueId > 0 ? sourceItem.UniqueId : null
					}
				}
			});
			stackResponses.Add(new ItemStackResponseContainerInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) destination.fullContainerName.containerName},
				slots = new List<ItemStackResponseSlotInfo>
				{
					new ItemStackResponseSlotInfo()
					{
						amount = destItem.Count,
						requestedSlot = destination.slot,
						slot = destination.slot,
						itemStackNetId = destItem.UniqueId > 0 ? destItem.UniqueId : null
					}
				}
			});
		}

		protected virtual void ProcessCraftResultDeprecatedAction(ItemStackRequestCraftResultsDeprecatedAction action)
		{
			// The client's own claim about what it crafted. Whenever the request named a recipe, the
			// registry already produced the output (ProcessCraftAction) and this claim is ignored.
			if (_activeRecipe != null) return;

			//BUG: Won't work proper with anvil anymore.
			if (_player.GetContainerItem(FullContainerName.ContainerEnumName.CursorContainer, 50).UniqueId > 0) return;

			//TODO: We only use this for anvils right now. Until we fixed the anvil merge ourselves.
			// The deprecated action carries a descriptor rather than an item, so the name has to be
			// resolved before it can be put in a slot. Only the name form can be, which is what an
			// anvil sends; anything else is refused rather than guessed at.
			ItemStackRequestNetworkItemInstanceDescriptor craftingResult = action.craftResults.FirstOrDefault();
			if (craftingResult?.itemDescriptor is not ItemNameDescriptor named)
			{
				if (craftingResult != null) Log.Warn($"Deprecated craft result carried {craftingResult.itemDescriptor?.GetType().Name ?? "nothing"}, which cannot name an item");
				return;
			}

			Item result = ItemFactory.GetItemByName(named.fullName, (short) named.auxValue);
			result.Count = (byte) craftingResult.stackSize;
			result.UniqueId = Environment.TickCount;
			_player.SetContainerItem(FullContainerName.ContainerEnumName.CursorContainer, 50, result);
		}

		protected virtual void ProcessCraftNotImplementedDeprecatedAction(ItemStackRequestCraftNonImplementedDeprecatedAction action)
		{
		}

		/// <summary>
		///     The recipe resolved for the request currently being handled, or null when the request named
		///     no recipe (creative pick, anvil merge, loom, ...).
		/// </summary>
		protected Recipe _activeRecipe;

		protected virtual void ProcessCraftAction(ItemStackRequestCraftRecipeAction action)
		{
			_activeRecipe = SetRecipeResult(ResolveRecipe(action.recipeNetId));
		}

		// Recipe-book "craft all/auto": same resolution, same server-produced output. The ingredients the
		// client listed in the action are ignored; the Consume actions that follow do the consuming.
		protected virtual void ProcessCraftAutoAction(ItemStackRequestCraftRecipeAutoAction action)
		{
			_activeRecipe = SetRecipeResult(ResolveRecipe(action.recipeNetId));
		}

		// A recipe network id the server never published is a desync, or a crafting exploit attempt:
		// throwing lands in Player's item-stack error path, which rejects the request and resyncs the
		// client's inventory.
		private Recipe ResolveRecipe(uint recipeNetworkId)
		{
			if (!RecipeManager.TryGetByNetworkId((int) recipeNetworkId, out Recipe recipe))
			{
				throw new Exception($"Unknown recipe network id: {recipeNetworkId}");
			}

			return recipe;
		}

		// Puts the recipe's own output in the crafting result slot; the client's CraftResultsDeprecated
		// items are never the source of truth. Returns the recipe when the registry produced the output,
		// or null for recipe kinds MiNET has no server-side output for yet (multi recipes: map cloning,
		// banner patterns, firework assembly), which keep using the deprecated-result flow.
		private Recipe SetRecipeResult(Recipe recipe)
		{
			Item result = recipe switch
			{
				ShapedRecipe shaped => shaped.Result.FirstOrDefault(),
				ShapelessRecipe shapeless => shapeless.Result.FirstOrDefault(),
				SmeltingRecipe smelting => smelting.Result,
				SmithingTransformRecipe transform => transform.Result,
				_ => null
			};

			if (result == null || result.Count == 0) return null;

			// Item.Clone is a shallow copy, so the NBT has to be copied too - otherwise the item handed
			// to the player shares its component NBT with the registry's recipe.
			var craftingResult = (Item) result.Clone();
			if (result.ExtraData != null) craftingResult.ExtraData = (NbtCompound) result.ExtraData.Clone();
			craftingResult.UniqueId = Environment.TickCount;
			_player.SetContainerItem(FullContainerName.ContainerEnumName.CursorContainer, 50, craftingResult);

			return recipe;
		}

		protected virtual void ProcessCraftCreativeAction(ItemStackRequestCraftCreativeAction action)
		{
			// Creative entry ids are positional (index + 1), assigned by SendCreativeInventory
			// over the same InventoryUtils list; sender and resolver share one source.
			int index = (int) action.creativeItemNetId - 1;
			Item creativeItem = index >= 0 && index < InventoryUtils.CreativeInventoryItems.Count
				? InventoryUtils.CreativeInventoryItems[index]
				: null;
			if (creativeItem == null) throw new Exception($"Failed to find inventory item with unique id: {action.creativeItemNetId}");
			creativeItem = ItemFactory.GetItemByName(creativeItem.Name, creativeItem.Metadata);
			creativeItem.Count = (byte) creativeItem.MaxStackSize;
			creativeItem.UniqueId = Environment.TickCount;
			Log.Debug($"Creating {creativeItem}");
			_player.Inventory.UiInventory.Slots[50] = creativeItem;
		}

		protected virtual void ProcessCraftRecipeOptionalAction(ItemStackRequestCraftRecipeOptionalAction action)
		{
		}

		// Below: screens whose rules the server does not know yet. Accepting the action and leaving the
		// output to the client's own CraftResultsDeprecated claim is what makes the screen usable at
		// all, and it does mean the client decides what comes out of it. Refusing instead is not the
		// safe option it looks like: an unhandled action fails the whole request, so the screen would
		// reject every craft rather than produce a wrong one. Each of these is the hook a server that
		// wants the real rules overrides.

		/// <summary>Loom. The pattern the client picked, applied to the banner in the input slot.</summary>
		protected virtual void ProcessCraftLoomAction(ItemStackRequestCraftLoomAction action)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Loom craft of pattern {action.patternNameId} x{action.numCrafts} taken on the client's word");
		}

		/// <summary>Grindstone: strip the enchantments, merge the durability, pay out the experience.</summary>
		protected virtual void ProcessCraftRepairAndDisenchantAction(ItemStackRequestCraftRepairAndDisenchantAction action)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Grindstone craft of recipe {action.recipeNetId} at cost {action.repairCost} taken on the client's word");
		}

		/// <summary>Beacon: consume the payment and apply the two effects it bought.</summary>
		protected virtual void ProcessBeaconPaymentAction(ItemStackRequestBeaconPaymentAction action)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Beacon paid for effects {action.primaryEffectId} and {action.secondaryEffectId}, which are not applied");
		}

		/// <summary>Education Edition lab table.</summary>
		protected virtual void ProcessLabTableCombineAction(ItemStackRequestLabTableCombineAction action)
		{
		}

		/// <summary>Taking one of several outputs a craft produced. The output the client is claiming
		/// already reached the UI window through the deprecated result action.</summary>
		protected virtual void ProcessCreateAction(ItemStackRequestCreateAction action)
		{
		}

		/// <summary>The client's own durability prediction for the tool it just mined with. The server
		/// damages the tool itself, so there is nothing to do with the prediction.</summary>
		protected virtual void ProcessMineBlockAction(ItemStackRequestMineBlockAction action)
		{
		}
	}
}