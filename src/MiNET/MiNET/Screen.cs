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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using log4net;
using MiNET.Net;
using MiNET.Utils.Vectors;
using Container = MiNET.Net.FullContainerName.ContainerEnumName;

namespace MiNET
{
	/// <summary>The screen the client is told to draw, sent as the type of ContainerOpen and echoed
	/// back in ContainerClose. Signed on the wire (NONE is -9, INVENTORY is -1) against a byte field,
	/// so the two negative ones are written here as the bytes they become.
	/// <para>Names and order are Mojang's ContainerType.json. That schema spells no numbers, so the
	/// values are its own order counted from CONTAINER, which agrees with every type MiNET already had
	/// working: chest 0, enchanting 3, anvil 5, beacon 13, loom 24, blast furnace 27.</para></summary>
	public enum ContainerType : byte
	{
		Container = 0,
		Workbench = 1,
		Furnace = 2,
		Enchantment = 3,
		BrewingStand = 4,
		Anvil = 5,
		Dispenser = 6,
		Dropper = 7,
		Hopper = 8,
		Cauldron = 9,
		MinecartChest = 10,
		MinecartHopper = 11,
		Horse = 12,
		Beacon = 13,
		StructureEditor = 14,
		Trade = 15,
		CommandBlock = 16,
		Jukebox = 17,
		Armor = 18,
		Hand = 19,
		CompoundCreator = 20,
		ElementConstructor = 21,
		MaterialReducer = 22,
		LabTable = 23,
		Loom = 24,
		Lectern = 25,
		Grindstone = 26,
		BlastFurnace = 27,
		Smoker = 28,
		Stonecutter = 29,
		Cartography = 30,
		Hud = 31,
		JigsawEditor = 32,
		SmithingTable = 33,
		ChestBoat = 34,
		DecoratedPot = 35,
		Crafter = 36,

		/// <summary>-9. What BDS echoes in the ContainerClose it answers with.</summary>
		None = 0xf7,

		/// <summary>-1. The player's own screen.</summary>
		Inventory = 0xff
	}

	/// <summary>What a player has open. One screen at a time, and there is always one: closing
	/// everything leaves <see cref="ScreenKind.Inventory" />.</summary>
	public enum ScreenKind
	{
		/// <summary>The player's own inventory. Cursor and the 2x2 grid, no block behind it.</summary>
		Inventory,

		/// <summary>Chest, shulker box, barrel, hopper, dispenser. Storage that lives in the block
		/// entity, shared by everyone looking at it.</summary>
		Container,

		Furnace,
		BlastFurnace,
		Smoker,
		BrewingStand,
		EnchantingTable,
		Anvil,
		Horse,

		/// <summary>Screens the client drives on its own: every slot is scratch in the flat UI window,
		/// nothing is stored server-side, and two players at one block never see each other's work.</summary>
		Workbench,

		Beacon,
		Loom,
		Grindstone,
		Stonecutter,
		Cartography,
		SmithingTable
	}

	/// <summary>Where a container name's items actually live.</summary>
	public enum SlotStore
	{
		/// <summary>The flat UI window (0x7c). Client-side scratch with no persistence: crafting
		/// grid, anvil and smithing inputs, enchanting, loom, grindstone, stonecutter, cartography,
		/// the cursor and the created output. Addressed by absolute index into that one window,
		/// which is why several container names share it.</summary>
		Ui,

		/// <summary>The player's own 45 slots, hotbar first.</summary>
		Main,

		/// <summary>Helmet, chest, leggings, boots.</summary>
		Armor,

		Offhand,

		/// <summary>The open screen's block inventory.</summary>
		Block,

		/// <summary>Named by the client but with nowhere to put the items. Addressing one is an
		/// error, never a silent drop.</summary>
		Unsupported
	}

	/// <summary>A container name resolved against the open screen: which store, and which index
	/// within it.</summary>
	public readonly struct SlotBinding
	{
		public SlotStore Store { get; }
		public int Index { get; }

		public SlotBinding(SlotStore store, int index)
		{
			Store = store;
			Index = index;
		}
	}

	public class Screen
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Screen));

		/// <summary>Container name to store. A single-slot container carries its index here, because
		/// the name alone fixes the slot and the client's slot number is then redundant. A
		/// multi-slot container has no index and passes the client's slot through.</summary>
		private static readonly Dictionary<Container, SlotBinding> Bindings = new Dictionary<Container, SlotBinding>
		{
			// The player's own storage. Present whatever screen is open.
			[Container.CombinedHotbarAndInventoryContainer] = new SlotBinding(SlotStore.Main, -1),
			[Container.HotbarContainer] = new SlotBinding(SlotStore.Main, -1),
			[Container.InventoryContainer] = new SlotBinding(SlotStore.Main, -1),
			[Container.ArmorContainer] = new SlotBinding(SlotStore.Armor, -1),
			[Container.OffhandContainer] = new SlotBinding(SlotStore.Offhand, 0),

			// Block storage, addressed by absolute slot in the block entity's item list.
			[Container.LevelEntityContainer] = new SlotBinding(SlotStore.Block, -1),
			[Container.ShulkerBoxContainer] = new SlotBinding(SlotStore.Block, -1),
			[Container.BarrelContainer] = new SlotBinding(SlotStore.Block, -1),
			[Container.CrafterLevelEntityContainer] = new SlotBinding(SlotStore.Block, -1),

			// Brewing stand. Three names over one five-slot block entity, and which slot each name
			// counts from is unverified: the client's own numbering is taken as given, so a bottle
			// landing in the ingredient slot is what tells us the layout, rather than a silent write
			// to a slot we guessed at.
			[Container.BrewingStandInputContainer] = new SlotBinding(SlotStore.Block, -1),
			[Container.BrewingStandFuelContainer] = new SlotBinding(SlotStore.Block, -1),
			[Container.BrewingStandResultContainer] = new SlotBinding(SlotStore.Block, -1),

			// Furnace family. Each name is one slot, and the index is the block entity's own
			// ordering: 0 smelts, 1 burns, 2 holds the result.
			[Container.FurnaceIngredientContainer] = new SlotBinding(SlotStore.Block, 0),
			[Container.BlastFurnaceIngredientContainer] = new SlotBinding(SlotStore.Block, 0),
			[Container.SmokerIngredientContainer] = new SlotBinding(SlotStore.Block, 0),
			[Container.FurnaceFuelContainer] = new SlotBinding(SlotStore.Block, 1),
			[Container.FurnaceResultContainer] = new SlotBinding(SlotStore.Block, 2),

			// Everything below is the flat UI window. None of it persists and none of it is shared,
			// so two players at one anvil never collide.
			[Container.CursorContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CreatedOutputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CraftingInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CraftingOutputPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.AnvilInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.AnvilMaterialContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.AnvilResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.SmithingTableInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.SmithingTableMaterialContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.SmithingTableTemplateContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.SmithingTableResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.EnchantingInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.EnchantingMaterialContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.GrindstoneInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.GrindstoneAdditionalContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.GrindstoneResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.StonecutterInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.StonecutterResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.LoomInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.LoomDyeContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.LoomMaterialContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.LoomResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CartographyInputContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CartographyAdditionalContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.CartographyResultPreviewContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeConstructionContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeNatureContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeItemsContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeSearchContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeSearchBarContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeEquipmentContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeBookContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeFoodContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeBlocksContainer] = new SlotBinding(SlotStore.Ui, -1),
			[Container.RecipeFurnaceItemsContainer] = new SlotBinding(SlotStore.Ui, -1),

			// The beacon's payment slot is scratch like the rest of the UI window: the ingot is never
			// stored in the block entity, it is consumed when the effect is chosen.
			[Container.BeaconPaymentContainer] = new SlotBinding(SlotStore.Ui, -1),

			// Screens whose contents have to be stored server-side, and are not. Listed so the
			// reason is "no storage yet" rather than "name unknown".
			[Container.HorseEquipContainer] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.DynamicContainer] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.TradeIngredient1Container] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.TradeIngredient2Container] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.TradeResultPreviewContainer] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.Trade2Ingredient1Container] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.Trade2Ingredient2Container] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.Trade2ResultPreviewContainer] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.CompoundCreatorInput] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.CompoundCreatorOutputPreview] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.ElementConstructorOutputPreview] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.MaterialReducerInput] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.MaterialReducerOutput] = new SlotBinding(SlotStore.Unsupported, -1),
			[Container.LabTableInput] = new SlotBinding(SlotStore.Unsupported, -1)
		};

		public ScreenKind Kind { get; }

		/// <summary>What the client was told to draw, and what it echoes back when it closes.</summary>
		public ContainerType Type { get; }

		/// <summary>The window id the client was handed. Every answer about this screen carries it,
		/// which is why it is held here and not read back off the inventory: a screen with no storage
		/// still has one.</summary>
		public byte WindowId { get; }

		/// <summary>The block the screen belongs to, or <see cref="BlockCoordinates.Zero" /> for a
		/// screen with no block (the player's own inventory, a horse).</summary>
		public BlockCoordinates Coordinates { get; }

		/// <summary>Storage behind the screen, shared with every other player looking at it. Null for
		/// a screen whose slots are all scratch.</summary>
		public IInventory Backing { get; }

		public Inventory BlockInventory => Backing as Inventory;

		public Screen(ScreenKind kind) : this(kind, ContainerType.Inventory, 0, BlockCoordinates.Zero, null)
		{
		}

		public Screen(ScreenKind kind, IInventory backing) : this(kind, ContainerType.None, 0, BlockCoordinates.Zero, backing)
		{
		}

		public Screen(ScreenKind kind, ContainerType type, byte windowId, BlockCoordinates coordinates, IInventory backing)
		{
			Kind = kind;
			Type = type;
			WindowId = windowId;
			Coordinates = coordinates;
			Backing = backing;
		}

		/// <summary>Resolves a container name and the slot the client sent into the store that holds
		/// the item. Throws rather than guessing: an unresolvable container gets an error response and
		/// a resync, where returning null would make the item vanish client-side.</summary>
		public SlotBinding Bind(Container container, int slot)
		{
			if (!Bindings.TryGetValue(container, out SlotBinding binding))
			{
				throw new InvalidOperationException($"Unknown container: {container}");
			}

			switch (binding.Store)
			{
				case SlotStore.Unsupported:
					throw new InvalidOperationException($"Container {container} has no server-side storage");
				case SlotStore.Block when BlockInventory == null:
					throw new InvalidOperationException($"Container {container} addressed with no block inventory open (screen is {Kind})");
			}

			if (binding.Index < 0) return new SlotBinding(binding.Store, slot);

			// A single-slot container is addressed by name, so the client's slot number is redundant
			// and ignored. Whether the client counts within the container or across the window is
			// unverified, and this is the line that settles it the first time one is clicked.
			if (slot != binding.Index && Log.IsDebugEnabled) Log.Debug($"Container {container} is fixed to slot {binding.Index}, client addressed it as {slot}");

			return binding;
		}
	}
}
