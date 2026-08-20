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
