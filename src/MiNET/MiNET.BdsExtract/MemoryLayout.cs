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
	///         Measured from the server before the sweep runs, never compiled in: this and the two
	///         pointers below moved between 1.26.3.1 and 1.26.30, and a build whose numbers differ
	///         finds no blocks at all. There is no fallback; reading an offset the measurement did
	///         not settle throws instead of producing a number nothing confirmed.
	///     </para>
	/// </summary>
	public static int NameInsideLegacy => _nameInsideLegacy ?? Missing(nameof(NameInsideLegacy));
	private static int? _nameInsideLegacy;

	/// <summary>
	///     Takes the block layout this server actually has. Applied only when the round trip that
	///     measures it closed on enough blocks to mean something; the caller decides that, and says
	///     out loud what it applied.
	/// </summary>
	public static void UseMeasured(int nameInsideLegacy, int defaultStatePointer, int blockLegacyPointer)
	{
		_nameInsideLegacy = nameInsideLegacy;
		_defaultStatePointer = defaultStatePointer;
		_blockLegacyPointer = blockLegacyPointer;
	}

	/// <summary>
	///     Takes a value's position as measured on this server. The name is the one the extraction
	///     writes the value under, so what was measured and what is applied cannot drift apart.
	/// </summary>
	public static bool UseMeasuredField(string field, int at)
	{
		switch (field)
		{
			case "hardness": _blockHardness = at; return true;
			case "explosionResistance": _blockExplosionResistance = at; return true;
			case "friction": _blockFriction = at; return true;
			case "burnOdds": _blockBurnOdds = at; return true;
			case "flameOdds": _blockFlameOdds = at; return true;
			case "isSolid": _blockIsSolid = at; return true;
			case "blockLightEmission": _blockLightEmissionAt = at; return true;
			case "blockLightDampening": _blockLightDampeningAt = at; return true;
			case "creativeCategory": _creativeCategoryAt = at; return true;
			case "solid": _blockSolidAt = at; return true;
			case "stateNbt": _blockStateNbt = at; return true;
			case "properties": _blockProperties = at; return true;
			case "components": _blockComponents = at; return true;
			case "componentIds": _blockComponentIds = at; return true;
			case "tags": _blockTags = at; return true;
			case "tagStride": _tagStride = at; return true;
			case "blockEntityType": _blockEntityTypeAt = at; return true;
			case "material": _materialAt = at; return true;
			case "fallable": _fallableAt = at >> 3; _fallableBit = at & 7; return true;
			case "requiresCorrectToolForDrops": _toolRequiredAt = at >> 3; _toolRequiredBit = at & 7; return true;
			case "lightEmission": _stateLightEmission = at; return true;
			case "lightDampening": _stateLightDampening = at; return true;
			case "translucency": _translucency = at; return true;
			case "thickness": _thickness = at; return true;
			case "states": _blockStates = at; return true;
			case "networkId": _blockNetworkId = at; return true;
			case "mapColor": _mapColor = at; return true;
			case "tintMethod": _tintMethod = at; return true;
			case "legacyId": _legacyId = at; return true;
			case "canContainLiquidSource": _blockCanContainLiquid = at; return true;
			case "liquidReactionOnTouch": _blockLiquidReaction = at; return true;
			case "serializationId": _serializationId = at; return true;
			case "creativeGroup": _creativeGroup = at; return true;
			default: return false;
		}
	}

	/// <summary>An offset nothing measured on this server. Reading it is refused, never answered.</summary>
	private static int Missing(string field) =>
		throw new InvalidOperationException($"{field} was never measured on this server");

	// BlockLegacy, measured from its name.
	public static int Thickness => _thickness ?? Missing(nameof(Thickness));
	private static int? _thickness;
	public static int Translucency => _translucency ?? Missing(nameof(Translucency));
	private static int? _translucency;
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
	public static int BlockSolidAt => _blockSolidAt ?? Missing(nameof(BlockSolidAt));
	private static int? _blockSolidAt;

	/// <summary>
	///     Whether the block drops nothing unless mined with the right tool, which is a different
	///     question from which tool is fastest: the tool tags say stone is quicker with a pickaxe
	///     AND that dirt is quicker with a shovel, and only stone withholds its drop.
	/// </summary>
	public static int ToolRequiredAt => _toolRequiredAt ?? Missing(nameof(ToolRequiredAt));
	private static int? _toolRequiredAt;

	public static int ToolRequiredBit => _toolRequiredBit ?? Missing(nameof(ToolRequiredBit));
	private static int? _toolRequiredBit;

	/// <summary>
	///     Whether the block falls when unsupported. Anvils, every concrete powder, gravel, sand
	///     and the dragon egg, and it agrees with CloudburstMC's independently produced
	///     block_attributes.json on all 496 blocks the two have in common.
	/// </summary>
	public static int FallableAt => _fallableAt ?? Missing(nameof(FallableAt));
	private static int? _fallableAt;

	public static int FallableBit => _fallableBit ?? Missing(nameof(FallableBit));
	private static int? _fallableBit;

	/// <summary>
	///     The block's own light values, emission and dampening outright rather than flags.
	///     These are the block's defaults and the state objects carry their own, so the two
	///     disagree exactly where light depends on state: campfire, soul_campfire and vault read
	///     0 here and 15, 10 and 6 on their lit states. Both are kept, because "what the block
	///     says" and "what this state emits" are different facts.
	/// </summary>
	public static int BlockLightEmissionAt => _blockLightEmissionAt ?? Missing(nameof(BlockLightEmissionAt));
	private static int? _blockLightEmissionAt;

	public static int BlockLightDampeningAt => _blockLightDampeningAt ?? Missing(nameof(BlockLightDampeningAt));
	private static int? _blockLightDampeningAt;

	/// <summary>
	///     Which block entity the block has, or zero for the 1,269 that have none.
	///     The grouping is its own proof: every chest variant reads 2, every sign 4, every shulker
	///     box 25, every hanging sign 50, every shelf 59, every copper golem statue 60.
	/// </summary>
	public static int BlockEntityTypeAt => _blockEntityTypeAt ?? Missing(nameof(BlockEntityTypeAt));
	private static int? _blockEntityTypeAt;

	/// <summary>
	///     The creative menu category, of which <see cref="CreativeGroup" /> is the group inside it.
	///     Emitted as the number rather than a name, because naming 3 and 6 would be guessing at
	///     two values nothing here pins down.
	/// </summary>
	public static int CreativeCategoryAt => _creativeCategoryAt ?? Missing(nameof(CreativeCategoryAt));
	private static int? _creativeCategoryAt;

	/// <summary>
	///     The block's material, which the server sets per block type in its constructor: 251 of
	///     the 256 classes hold one value, and the five that vary do so because the class takes it
	///     as an argument, which is how water and lava share a class and read 3 and 5.
	/// </summary>
	public static int MaterialAt => _materialAt ?? Missing(nameof(MaterialAt));
	private static int? _materialAt;

	public static int MapColor => _mapColor ?? Missing(nameof(MapColor));   // four floats: red, green, blue, alpha
	private static int? _mapColor;
	public static int TintMethod => _tintMethod ?? Missing(nameof(TintMethod));
	private static int? _tintMethod;
	public static int LegacyId => _legacyId ?? Missing(nameof(LegacyId));   // the pre-flattening numeric id
	private static int? _legacyId;
	public static int DefaultStatePointer => _defaultStatePointer ?? Missing(nameof(DefaultStatePointer));
	private static int? _defaultStatePointer;
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
	public static int BlockIsSolid => _blockIsSolid ?? Missing(nameof(BlockIsSolid));
	private static int? _blockIsSolid;

	public static int BlockFlameOdds => _blockFlameOdds ?? Missing(nameof(BlockFlameOdds));
	private static int? _blockFlameOdds;
	public static int BlockBurnOdds => _blockBurnOdds ?? Missing(nameof(BlockBurnOdds));
	private static int? _blockBurnOdds;
	public static int BlockExplosionResistance => _blockExplosionResistance ?? Missing(nameof(BlockExplosionResistance));
	private static int? _blockExplosionResistance;
	public static int BlockFriction => _blockFriction ?? Missing(nameof(BlockFriction));
	private static int? _blockFriction;
	public static int BlockHardness => _blockHardness ?? Missing(nameof(BlockHardness));
	private static int? _blockHardness;
	public static int BlockCanContainLiquid => _blockCanContainLiquid ?? Missing(nameof(BlockCanContainLiquid));
	private static int? _blockCanContainLiquid;
	public static int BlockLiquidReaction => _blockLiquidReaction ?? Missing(nameof(BlockLiquidReaction));
	private static int? _blockLiquidReaction;

	// Block, the per-state object, measured from its own start.
	public static int BlockLegacyPointer => _blockLegacyPointer ?? Missing(nameof(BlockLegacyPointer));
	private static int? _blockLegacyPointer;

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
	public static int BlockStateNbt => _blockStateNbt ?? Missing(nameof(BlockStateNbt));
	private static int? _blockStateNbt;

	// One node of that map, measured from its own start.
	public const int MapNodeLeft = 0;
	public const int MapNodeParent = 8;      // on the head sentinel this is the real root
	public const int MapNodeRight = 16;
	public const int MapNodeFlags = 24;      // colour, then a byte marking the sentinel itself
	public const int MapNodeKey = 32;        // std::string
	public const int MapNodeVtable = 64;     // which tag the value is
	public const int MapNodePayload = 72;
	public const int MapNodeSize = 128;

	public static int StateLightEmission => _stateLightEmission ?? Missing(nameof(StateLightEmission));
	private static int? _stateLightEmission;

	public static int StateLightDampening => _stateLightDampening ?? Missing(nameof(StateLightDampening));
	private static int? _stateLightDampening;
	public static int BlockNetworkId => _blockNetworkId ?? Missing(nameof(BlockNetworkId));
	private static int? _blockNetworkId;
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
	public static int SerializationId => _serializationId ?? Missing(nameof(SerializationId));
	private static int? _serializationId;
	public static int CreativeGroup => _creativeGroup ?? Missing(nameof(CreativeGroup));
	private static int? _creativeGroup;

	/// <summary>
	///     The block's states indexed by the old data value, also from the object start. Not the
	///     palette's list: it is the full product of each property's bit width, so it holds a slot
	///     for every data value the four bits could carry and leaves the illegal ones null.
	/// </summary>
	public static int BlockStates => _blockStates ?? Missing(nameof(BlockStates));
	private static int? _blockStates;

	/// <summary>
	///     The block's own state properties, a sixteen bucket hash on BlockLegacy. Each entry is a
	///     list node, links then the property's name as a HashedString at +16 and the server's
	///     number for it at +64. That number is the same on every block carrying the property.
	/// </summary>
	public static int BlockProperties => _blockProperties ?? Missing(nameof(BlockProperties));
	private static int? _blockProperties;

	/// <summary>
	///     The block's components, a vector of pointers on BlockLegacy. Each is a method table then
	///     the component's own data, and the one carrying a namespaced std::string at +16 is the
	///     geometry. Only a block that overrides its shape has one; the rest take the shape of the
	///     class implementing them.
	/// </summary>
	public static int BlockComponents => _blockComponents ?? Missing(nameof(BlockComponents));
	private static int? _blockComponents;
	public const int ComponentText = 16;

	/// <summary>
	///     One sixteen bit id per component, in a vector running in step with the pointers above.
	///     This is what a component IS. The method table cannot say: one class serves several
	///     different components, so grouping on it puts two of them in one bucket, and recognising
	///     a component by the value it holds mistakes any component holding a similar value for it.
	///     The id does neither. Across all 1,429 blocks not one id covers two components.
	/// </summary>
	public static int BlockComponentIds => _blockComponentIds ?? Missing(nameof(BlockComponentIds));
	private static int? _blockComponentIds;

	/// <summary>
	///     The block's tags: a vector of HashedString on BlockLegacy, so they belong to the block
	///     and every one of its states shares them. The state objects hold no tag list of their own.
	///     Elements are forty eight bytes, a HashedString of forty and eight of padding.
	/// </summary>
	public static int BlockTags => _blockTags ?? Missing(nameof(BlockTags));
	private static int? _blockTags;
	public static int TagStride => _tagStride ?? Missing(nameof(TagStride));
	private static int? _tagStride;

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
