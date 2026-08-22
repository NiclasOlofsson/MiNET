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
	/// <summary>
	///     How far the name sits inside BlockLegacy. Subtract to get the object start.
	///     <para>
	///         Measured from the server before the sweep runs, not fixed: this and the two pointers
	///         below moved between 1.26.3.1 and 1.26.30, and a build whose numbers differ finds no
	///         blocks at all. The value here is what the builds we target hold, kept as the answer
	///         to fall back on when the measurement cannot be made.
	///     </para>
	/// </summary>
	public static int NameInsideLegacy { get; private set; } = 224;

	/// <summary>
	///     Takes the block layout this server actually has. Applied only when the round trip that
	///     measures it closed on enough blocks to mean something; the caller decides that, and says
	///     out loud what it applied.
	/// </summary>
	public static void UseMeasured(int nameInsideLegacy, int defaultStatePointer, int blockLegacyPointer)
	{
		NameInsideLegacy = nameInsideLegacy;
		DefaultStatePointer = defaultStatePointer;
		BlockLegacyPointer = blockLegacyPointer;
	}

	/// <summary>
	///     Takes a value's position as measured on this server. The name is the one the extraction
	///     writes the value under, so what was measured and what is applied cannot drift apart.
	/// </summary>
	public static bool UseMeasuredField(string field, int at)
	{
		switch (field)
		{
			case "hardness": BlockHardness = at; return true;
			case "explosionResistance": BlockExplosionResistance = at; return true;
			case "friction": BlockFriction = at; return true;
			case "burnOdds": BlockBurnOdds = at; return true;
			case "flameOdds": BlockFlameOdds = at; return true;
			case "isSolid": BlockIsSolid = at; return true;
			case "blockLightEmission": BlockLightEmissionAt = at; return true;
			case "blockLightDampening": BlockLightDampeningAt = at; return true;
			case "creativeCategory": CreativeCategoryAt = at; return true;
			case "solid": BlockSolidAt = at; return true;
			case "stateNbt": BlockStateNbt = at; return true;
			case "properties": BlockProperties = at; return true;
			case "components": BlockComponents = at; return true;
			case "componentIds": BlockComponentIds = at; return true;
			case "tags": BlockTags = at; return true;
			case "tagStride": TagStride = at; return true;
			case "blockEntityType": BlockEntityTypeAt = at; return true;
			case "material": MaterialAt = at; return true;
			case "fallable": FallableAt = at >> 3; FallableBit = at & 7; return true;
			case "requiresCorrectToolForDrops": ToolRequiredAt = at >> 3; ToolRequiredBit = at & 7; return true;
			case "lightEmission": StateLightEmission = at; return true;
			case "lightDampening": StateLightDampening = at; return true;
			case "translucency": Translucency = at; return true;
			case "thickness": Thickness = at; return true;
			case "states": BlockStates = at; return true;
			case "networkId": BlockNetworkId = at; return true;
			case "mapColor": MapColor = at; return true;
			case "tintMethod": TintMethod = at; return true;
			case "legacyId": LegacyId = at; return true;
			case "canContainLiquidSource": BlockCanContainLiquid = at; return true;
			case "liquidReactionOnTouch": BlockLiquidReaction = at; return true;
			case "serializationId": SerializationId = at; return true;
			case "creativeGroup": CreativeGroup = at; return true;
			default: return false;
		}
	}

	// BlockLegacy, measured from its name.
	public static int Thickness { get; private set; } = 120;
	public static int Translucency { get; private set; } = 124;
	/// <summary>
	///     Eight bytes between the translucency float and the map colour, read because they are
	///     inside the object and skipping them is loss, not economy. Five of them have their own
	///     getter in the method table, which is what says they are fields rather than padding.
	///     One is identified: bit 1 of name+132 is the solid flag, and it agrees with the state
	///     object's own isSolid on all 1,429 blocks. The rest are emitted by offset with no name,
	///     because a byte whose meaning is unknown is still a byte that was there.
	/// </summary>
	/// <summary>
	///     The eight bytes between the translucency float and the map colour. Every one of them is
	///     named below and located against the reference, so the span itself is derived from those
	///     positions rather than stated: it runs from the first named byte to the last. It is still
	///     emitted whole, because the bits inside three of them have no name yet and a byte whose
	///     meaning is unknown is still a byte that was there.
	/// </summary>
	public static int UnnamedBytes => Math.Min(CreativeCategoryAt, Math.Min(BlockEntityTypeAt, Math.Min(MaterialAt,
		Math.Min(FallableAt, Math.Min(ToolRequiredAt,
			Math.Min(BlockLightDampeningAt, BlockLightEmissionAt))))));

	public static int UnnamedByteCount => Math.Max(CreativeCategoryAt, Math.Max(BlockEntityTypeAt, Math.Max(MaterialAt,
		Math.Max(FallableAt, Math.Max(ToolRequiredAt,
			Math.Max(BlockLightDampeningAt, BlockLightEmissionAt)))))) - UnnamedBytes + 1;

	/// <summary>
	///     The block's own solid flag, a whole byte of its own. Read here as well as on the state,
	///     because they are two fields on two objects and only measuring both says whether they
	///     agree. It was previously read as bit 1 of name+132, which BlockType says is a byte of the
	///     particle quantity float, and nothing used that read.
	/// </summary>
	public static int BlockSolidAt { get; private set; } = 180;

	/// <summary>
	///     Whether the block drops nothing unless mined with the right tool, which is a different
	///     question from which tool is fastest: the tool tags say stone is quicker with a pickaxe
	///     AND that dirt is quicker with a shovel, and only stone withholds its drop.
	/// </summary>
	public static int ToolRequiredAt { get; private set; } = 133;

	public static int ToolRequiredBit { get; private set; } = 2;

	/// <summary>
	///     Whether the block falls when unsupported. Anvils, every concrete powder, gravel, sand
	///     and the dragon egg, and it agrees with CloudburstMC's independently produced
	///     block_attributes.json on all 496 blocks the two have in common.
	/// </summary>
	public static int FallableAt { get; private set; } = 131;

	public static int FallableBit { get; private set; } = 4;

	/// <summary>
	///     The block's own light values, emission and dampening outright rather than flags.
	///     These are the block's defaults and the state objects carry their own, so the two
	///     disagree exactly where light depends on state: campfire, soul_campfire and vault read
	///     0 here and 15, 10 and 6 on their lit states. Both are kept, because "what the block
	///     says" and "what this state emits" are different facts.
	/// </summary>
	public static int BlockLightEmissionAt { get; private set; } = 135;

	public static int BlockLightDampeningAt { get; private set; } = 134;

	/// <summary>
	///     Which block entity the block has, or zero for the 1,269 that have none.
	///     The grouping is its own proof: every chest variant reads 2, every sign 4, every shulker
	///     box 25, every hanging sign 50, every shelf 59, every copper golem statue 60.
	/// </summary>
	public static int BlockEntityTypeAt { get; private set; } = 129;

	/// <summary>
	///     The creative menu category, of which <see cref="CreativeGroup" /> is the group inside it.
	///     Emitted as the number rather than a name, because naming 3 and 6 would be guessing at
	///     two values nothing here pins down.
	/// </summary>
	public static int CreativeCategoryAt { get; private set; } = 128;

	/// <summary>
	///     The block's material, which the server sets per block type in its constructor: 251 of
	///     the 256 classes hold one value, and the five that vary do so because the class takes it
	///     as an argument, which is how water and lava share a class and read 3 and 5.
	/// </summary>
	public static int MaterialAt { get; private set; } = 130;

	public static int MapColor { get; private set; } = 136;   // four floats: red, green, blue, alpha
	public static int TintMethod { get; private set; } = 156;
	public static int LegacyId { get; private set; } = 158;   // the pre-flattening numeric id
	public static int DefaultStatePointer { get; private set; } = 352;
	/// <summary>How far past the name is ever read, which is as far as the furthest field there reaches.</summary>
	public static int LegacyReach => Round(Max(
		DefaultStatePointer + 8, MapColor + 16, TintMethod + 1, LegacyId + 2,
		Thickness + 4, Translucency + 4, BlockTags + 24,
		UnnamedBytes + UnnamedByteCount, HashedString.Size));

	private static int Max(params int[] values) => values.Max();

	/// <summary>Up to the next eight, because everything here is read a word at a time.</summary>
	private static int Round(int value) => (value + 7) / 8 * 8;

	/// <summary>
	///     Block fields that are the same for every state of a block, so the default state
	///     answers for all of them. Checked across all 17,700 states: none of these differ
	///     between the states of one block, unlike the two light fields below.
	/// </summary>
	public static int BlockIsSolid { get; private set; } = 113;

	public static int BlockFlameOdds { get; private set; } = 168;
	public static int BlockBurnOdds { get; private set; } = 170;
	public static int BlockExplosionResistance { get; private set; } = 172;
	public static int BlockFriction { get; private set; } = 176;
	public static int BlockHardness { get; private set; } = 180;
	public static int BlockCanContainLiquid { get; private set; } = 184;
	public static int BlockLiquidReaction { get; private set; } = 186;

	// Block, the per-state object, measured from its own start.
	public static int BlockLegacyPointer { get; private set; } = 104;

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
	public static int BlockStateNbt { get; private set; } = 248;

	// One node of that map, measured from its own start.
	public const int MapNodeLeft = 0;
	public const int MapNodeParent = 8;      // on the head sentinel this is the real root
	public const int MapNodeRight = 16;
	public const int MapNodeFlags = 24;      // colour, then a byte marking the sentinel itself
	public const int MapNodeKey = 32;        // std::string
	public const int MapNodeVtable = 64;     // which tag the value is
	public const int MapNodePayload = 72;
	public const int MapNodeSize = 128;

	public static int StateLightEmission { get; private set; } = 164;

	public static int StateLightDampening { get; private set; } = 165;
	public static int BlockNetworkId { get; private set; } = 276;
	/// <summary>
	///     How much of a state object is read, which is as far as the furthest field measured in it
	///     reaches. Stating it as a number instead meant a build that moved a field further out read
	///     short and lost it silently.
	/// </summary>
	public static int BlockSize => Round(Max(
		BlockNetworkId + 4, BlockHardness + 4, BlockFriction + 4, BlockExplosionResistance + 4,
		BlockLegacyPointer + 8, BlockStateNbt + 8, BlockIsSolid + 1, BlockBurnOdds + 1,
		BlockFlameOdds + 1, BlockCanContainLiquid + 1, BlockLiquidReaction + 1,
		StateLightEmission + 1, StateLightDampening + 1));

	/// <summary>
	///     Two strings on BlockLegacy, measured from the START of the object rather than from the
	///     name, because they sit in front of it. The serialization id is the pre flattening name,
	///     tile.acacia_button, which every block has; the creative group is the tab it appears in,
	///     itemGroup.name.buttons, which only the blocks a player can reach have.
	/// </summary>
	public static int SerializationId { get; private set; } = 8;
	public static int CreativeGroup { get; private set; } = 312;

	/// <summary>
	///     The block's states indexed by the old data value, also from the object start. Not the
	///     palette's list: it is the full product of each property's bit width, so it holds a slot
	///     for every data value the four bits could carry and leaves the illegal ones null.
	/// </summary>
	public static int BlockStates { get; private set; } = 552;

	/// <summary>
	///     The block's own state properties, a sixteen bucket hash on BlockLegacy. Each entry is a
	///     list node, links then the property's name as a HashedString at +16 and the server's
	///     number for it at +64. That number is the same on every block carrying the property.
	/// </summary>
	public static int BlockProperties { get; private set; } = 512;

	/// <summary>
	///     The block's components, a vector of pointers on BlockLegacy. Each is a method table then
	///     the component's own data, and the one carrying a namespaced std::string at +16 is the
	///     geometry. Only a block that overrides its shape has one; the rest take the shape of the
	///     class implementing them.
	/// </summary>
	public static int BlockComponents { get; private set; } = 72;
	public const int ComponentText = 16;

	/// <summary>
	///     One sixteen bit id per component, in a vector running in step with the pointers above.
	///     This is what a component IS. The method table cannot say: one class serves several
	///     different components, so grouping on it puts two of them in one bucket, and recognising
	///     a component by the value it holds mistakes any component holding a similar value for it.
	///     The id does neither. Across all 1,429 blocks not one id covers two components.
	/// </summary>
	public static int BlockComponentIds { get; private set; } = 48;

	/// <summary>
	///     The block's tags: a vector of HashedString on BlockLegacy, so they belong to the block
	///     and every one of its states shares them. The state objects hold no tag list of their own.
	///     Elements are forty eight bytes, a HashedString of forty and eight of padding.
	/// </summary>
	public static int BlockTags { get; private set; } = 192;
	public static int TagStride { get; private set; } = 48;

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
