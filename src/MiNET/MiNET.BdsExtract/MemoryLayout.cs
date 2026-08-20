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
///     Three objects are involved:
///     BlockLegacy is the block, one per block name. Its name is a HashedString partway inside it,
///     and since that is what the memory sweep finds, every BlockLegacy field below is measured
///     from the name rather than from the object start.
///     Material is a second object BlockLegacy points at, holding what it takes to destroy the
///     block. Its fields are measured from its own start.
///     Block is a state, one per palette entry. Its fields are measured from its own start.
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
	public const int MaterialPointer = 352;
	public const int LegacyReach = 360;       // how far past the name we ever read

	// Material, measured from its own start.
	public const int MaterialIsSolid = 113;
	public const int MaterialLightEmission = 164;
	public const int MaterialLightDampening = 165;
	public const int MaterialFlameOdds = 168;
	public const int MaterialBurnOdds = 170;
	public const int MaterialExplosionResistance = 172;
	public const int MaterialFriction = 176;
	public const int MaterialHardness = 180;
	public const int MaterialCanContainLiquid = 184;
	public const int MaterialLiquidReaction = 186;
	public const int MaterialSize = 192;

	// Block, the per-state object, measured from its own start.
	public const int BlockLegacyPointer = 104;
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
