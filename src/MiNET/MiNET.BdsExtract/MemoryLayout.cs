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

namespace MiNET.BdsExtract;

/// <summary>
///     Where the values sit inside the server's block objects.
///     None of these were guessed. Each was found by taking blocks whose value is known, looking
///     at every position in memory around them, and keeping the one position that agreed with
///     every block at once. A wrong offset does not fit a thousand blocks by accident.
///     They are version specific. When a new server build moves a field the extraction does not
///     quietly produce wrong numbers: the checks in <see cref="ExtractionReport" /> fail, because
///     a name stops hashing to its own hash and the palette stops being ordered. Re-derive these
///     rather than nudging them until something compiles.
///     Two objects are involved, and their old names still show up in the wild: BlockLegacy used
///     to be Tile, and Block used to be Material.
///     BlockLegacy is the block, one per block name. Its name is a HashedString partway inside it,
///     and since that is what the memory sweep finds, every BlockLegacy field below is measured
///     from the name rather than from the object start.
///     Block is a state, one per palette entry, and its fields are measured from its own start.
///     BlockLegacy points at one of them, its default state, and that is where the per block
///     values are read from. Verified against every block: the address at name+352 is always a
///     palette entry, always one belonging to that same block, and shares its method table.
/// </summary>
public static class MemoryLayout
{
	/// <summary>How far the name sits inside BlockLegacy. Subtract to get the object start.</summary>
	public const int NameInsideLegacy = 224;

	// BlockLegacy, measured from its name.
	public const int Thickness = 120;
	public const int Translucency = 124;
	/// <summary>
	///     Eight bytes between the translucency float and the map colour, read because they are
	///     inside the object and skipping them is loss, not economy. Five of them have their own
	///     getter in the method table, which is what says they are fields rather than padding.
	///     One is identified: bit 1 of name+132 is the solid flag, and it agrees with the state
	///     object's own isSolid on all 1,429 blocks. The rest are emitted by offset with no name,
	///     because a byte whose meaning is unknown is still a byte that was there.
	/// </summary>
	public const int UnnamedBytes = 128;

	public const int UnnamedByteCount = 8;

	/// <summary>Which of the eight carries the solid flag, and in which bit.</summary>
	public const int SolidFlagByte = 4;

	public const int SolidFlagBit = 1;

	/// <summary>
	///     Whether the block drops nothing unless mined with the right tool, which is a different
	///     question from which tool is fastest: the tool tags say stone is quicker with a pickaxe
	///     AND that dirt is quicker with a shovel, and only stone withholds its drop.
	///     Identified against the rest of the extraction rather than by shape. Every one of the 118
	///     blocks carrying a minimum tier tag has it set, no axe, hoe or crop block has it, and the
	///     only two shovel blocks that do are snow and snow_layer, which is exactly right. The 85
	///     pickaxe blocks without it are rails, shulker boxes, bells and the like, mined faster
	///     with a pickaxe but dropping themselves either way.
	/// </summary>
	public const int ToolRequiredByte = 5;

	public const int ToolRequiredBit = 2;

	/// <summary>
	///     Whether the block falls when unsupported. Anvils, every concrete powder, gravel, sand
	///     and the dragon egg, and it agrees with CloudburstMC's independently produced
	///     block_attributes.json on all 496 blocks the two have in common.
	/// </summary>
	public const int FallableByte = 3;

	public const int FallableBit = 4;

	/// <summary>
	///     The block's own light values, which are not flags at all: the last two of the eight
	///     bytes are the emission and the dampening outright.
	///     These are the block's defaults and the state objects carry their own, so the two
	///     disagree exactly where light depends on state: campfire, soul_campfire and vault read
	///     0 here and 15, 10 and 6 on their lit states. Both are kept, because "what the block
	///     says" and "what this state emits" are different facts.
	/// </summary>
	public const int LightEmissionByte = 7;

	public const int LightDampeningByte = 6;

	/// <summary>
	///     Which block entity the block has, or zero for the 1,269 that have none.
	///     The grouping is its own proof: every chest variant reads 2, every sign 4, every shulker
	///     box 25, every hanging sign 50, every shelf 59, every copper golem statue 60. Fifty nine
	///     values, each one collecting exactly the blocks that share a block entity kind, which is
	///     not something a byte read at the wrong offset produces.
	/// </summary>
	public const int BlockEntityTypeByte = 1;

	/// <summary>
	///     The creative menu category, of which <see cref="CreativeGroup" /> is the group inside it.
	///     Seven values, and the groups nest correctly: 1 holds slabs, stairs, copper and
	///     trapdoors, 2 holds coral, logs, flowers and food, 4 holds shulker boxes, pressure plates
	///     and buttons, 5 holds the permission blocks. Emitted as the number rather than a name,
	///     because naming 3 and 6 would be guessing at two values nothing here pins down.
	/// </summary>
	public const int CreativeCategoryByte = 0;

	/// <summary>
	///     The block's material, which the server sets per block type in its constructor: 251 of
	///     the 256 classes hold one value, and the five that vary do so because the class takes it
	///     as an argument, which is how water and lava share a class and read 3 and 5.
	///     Named by the fifty data driven blocks BDS ships, whose JSON states it: the 48 declaring
	///     "solid" read 5 and the one declaring "plant" reads 11. The rest of the values are not
	///     named because no block declares them, so the number goes out on its own. Observed:
	///     3 for glass, panes, ice and water, 5 solid, 7 for torches and the newer leaves, 8 for
	///     bars, chains and coral fans, 10 for the six classic leaves, 11 plant, and one value each
	///     for the end portals, barrier, light blocks and structure void.
	/// </summary>
	public const int MaterialByte = 2;

	public const int MapColor = 136;          // four floats: red, green, blue, alpha
	public const int TintMethod = 156;
	public const int LegacyId = 158;          // the pre-flattening numeric id
	public const int DefaultStatePointer = 352;
	public const int LegacyReach = 360;       // how far past the name we ever read

	/// <summary>
	///     Block fields that are the same for every state of a block, so the default state
	///     answers for all of them. Checked across all 17,700 states: none of these differ
	///     between the states of one block, unlike the two light fields below.
	/// </summary>
	public const int BlockIsSolid = 113;

	public const int BlockFlameOdds = 168;
	public const int BlockBurnOdds = 170;
	public const int BlockExplosionResistance = 172;
	public const int BlockFriction = 176;
	public const int BlockHardness = 180;
	public const int BlockCanContainLiquid = 184;
	public const int BlockLiquidReaction = 186;

	// Block, the per-state object, measured from its own start.
	public const int BlockLegacyPointer = 104;

	/// <summary>
	///     Light is a property of the state, not of the block. A candle emits nothing unlit and
	///     3, 6, 9 or 12 lit depending on how many candles the state has; a respawn anchor runs
	///     0, 3, 7, 11, 15 across its charges; a cauldron dampens 3 empty and 14 full. Fifty one
	///     blocks disagree with themselves this way, so reading one value per block gets them
	///     wrong. Both are 0..15.
	/// </summary>
	/// <summary>
	///     The serialized NBT for this state: the compound of name, states and version that the
	///     network id is a hash of. It is an MSVC std::map, walked by <see cref="BlockStateReader" />.
	/// </summary>
	public const int BlockStateNbt = 248;

	// One node of that map, measured from its own start.
	public const int MapNodeLeft = 0;
	public const int MapNodeParent = 8;      // on the head sentinel this is the real root
	public const int MapNodeRight = 16;
	public const int MapNodeFlags = 24;      // colour, then a byte marking the sentinel itself
	public const int MapNodeKey = 32;        // std::string
	public const int MapNodeVtable = 64;     // which tag the value is
	public const int MapNodePayload = 72;
	public const int MapNodeSize = 128;

	public const int BlockLightEmission = 164;

	public const int BlockLightDampening = 165;
	public const int BlockNetworkId = 276;
	public const int BlockSize = 304;

	/// <summary>
	///     Two strings on BlockLegacy, measured from the START of the object rather than from the
	///     name, because they sit in front of it. The serialization id is the pre flattening name,
	///     tile.acacia_button, which every block has; the creative group is the tab it appears in,
	///     itemGroup.name.buttons, which only the blocks a player can reach have.
	/// </summary>
	public const int SerializationId = 8;
	public const int CreativeGroup = 312;

	/// <summary>
	///     The block's states indexed by the old data value, also from the object start. Not the
	///     palette's list: it is the full product of each property's bit width, so it holds a slot
	///     for every data value the four bits could carry and leaves the illegal ones null.
	/// </summary>
	public const int BlockStates = 552;

	/// <summary>
	///     The block's own state properties, a sixteen bucket hash on BlockLegacy. Each entry is a
	///     list node, links then the property's name as a HashedString at +16 and the server's
	///     number for it at +64. That number is the same on every block carrying the property.
	/// </summary>
	public const int BlockProperties = 512;

	/// <summary>
	///     The block's components, a vector of pointers on BlockLegacy. Each is a method table then
	///     the component's own data, and the one carrying a namespaced std::string at +16 is the
	///     geometry. Only a block that overrides its shape has one; the rest take the shape of the
	///     class implementing them.
	/// </summary>
	public const int BlockComponents = 72;
	public const int ComponentText = 16;

	/// <summary>
	///     One sixteen bit id per component, in a vector running in step with the pointers above.
	///     This is what a component IS. The method table cannot say: one class serves several
	///     different components, so grouping on it puts two of them in one bucket, and recognising
	///     a component by the value it holds mistakes any component holding a similar value for it.
	///     The id does neither. Across all 1,429 blocks not one id covers two components.
	/// </summary>
	public const int BlockComponentIds = 48;

	/// <summary>
	///     The block's tags: a vector of HashedString on BlockLegacy, so they belong to the block
	///     and every one of its states shares them. The state objects hold no tag list of their own.
	///     Elements are forty eight bytes, a HashedString of forty and eight of padding.
	/// </summary>
	public const int BlockTags = 192;
	public const int TagStride = 48;

	/// <summary>A tag has no namespace to require, and the shortest here is four characters.</summary>
	public const int MinimumTagLength = 3;
	public const int MaximumTagLength = 64;

	/// <summary>Values of the tint method byte, in the order the server numbers them.</summary>
	public static readonly string[] TintMethods =
	[
		"None", "DefaultFoliage", "BirchFoliage", "EvergreenFoliage", "DryFoliage",
		"Grass", "Water", "Stem", "RedStoneWire"
	];

	/// <summary>Values of the liquid reaction byte, in the order the server numbers them.</summary>
	public static readonly string[] LiquidReactions = ["BROKEN", "POPPED", "BLOCKING", "NOREACTION"];

	public static string Describe(string[] table, byte value)
	{
		return value < table.Length ? table[value] : $"Unknown{value}";
	}
}
