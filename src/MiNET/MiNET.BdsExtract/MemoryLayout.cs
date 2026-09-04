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
			case "stateNbt": _blockStateNbt = at; return true;
			case "properties": _blockProperties = at; return true;
			case "components": _blockComponents = at; return true;
			case "componentIds": _blockComponentIds = at; return true;
			case "lightEmission": _stateLightEmission = at; return true;
			case "lightDampening": _stateLightDampening = at; return true;
			case "states": _blockStates = at; return true;
			case "networkId": _blockNetworkId = at; return true;
			default: return false;
		}
	}

	/// <summary>An offset nothing measured on this server. Reading it is refused, never answered.</summary>
	private static int Missing(string field) =>
		throw new InvalidOperationException($"{field} was never measured on this server");

	// BlockLegacy, measured from its name.
	public static int DefaultStatePointer => _defaultStatePointer ?? Missing(nameof(DefaultStatePointer));
	private static int? _defaultStatePointer;

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
