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
///     Where an item keeps each of its values, stated from its name because that is the one place in
///     the object this tool can find without being told: the name verifies against its own hash.
///     Everything else is measured from it.
///     <para>
///         The numbers here are what the last checked build had. Every one of them that the item
///         file states a value for is measured again on each run against
///         Assets/reference-items.json, and what is measured replaces what is written here, so a
///         build that moved a field is read correctly rather than read at the old place.
///     </para>
/// </summary>
public static class ItemLayout
{
	/// <summary>The object's own start, which is where its method table sits.</summary>
	public static int MethodTable => -ItemRegistry.NameInsideItem;

	public static int ParseVersion { get; private set; } = -280;
	public static int TextureAtlas { get; private set; } = -272;
	public static int IconFrameCount { get; private set; } = -240;
	public static int AnimatesInToolbar { get; private set; } = -236;
	public static int MirroredArt { get; private set; } = -235;
	public static int UseAnimation { get; private set; } = -234;
	public static int HoverTextColour { get; private set; } = -232;
	public static int Icon { get; private set; } = -184;
	public static int SecondIcon { get; private set; } = -152;
	public static int MaxStackSize { get; private set; } = -120;
	public static int Id { get; private set; } = -118;
	public static int TranslationKey { get; private set; } = -112;
	public static int BareName { get; private set; } = -80;
	public static int Namespace { get; private set; } = -32;
	public static int MaxDurability { get; private set; } = 48;
	public static int Flags { get; private set; } = 50;
	public static int UseDuration { get; private set; } = 52;
	public static int MinimumVersion { get; private set; } = 56;
	public static int Block { get; private set; } = 88;
	public static int CreativeCategory { get; private set; } = 96;
	public static int CreativeGroup { get; private set; } = 112;
	public static int FurnaceFuel { get; private set; } = 144;
	public static int SmeltingExperience { get; private set; } = 148;
	public static int HiddenInCommands { get; private set; } = 152;
	public static int Rarity { get; private set; } = 156;
	public static int MineBlockType { get; private set; } = 160;
	public static int FoodComponent { get; private set; } = 168;
	public static int SeedComponent { get; private set; } = 176;
	public static int CameraComponent { get; private set; } = 184;

	/// <summary>The item left in the grid after crafting with this one. Null on every vanilla item.</summary>
	public static int CraftingRemainingItem { get; private set; } = 104;

	/// <summary>The second byte of the flag bitfield: ignoresPermissions in bit 0, seven bits of padding after it.</summary>
	public static int SecondFlags { get; private set; } = 51;
	/// <summary>
	///     The callbacks the item runs when its block AI is reset. Named "seed vector" here until
	///     Item's own declaration showed the seed component is the pointer at +176 and this is
	///     vector&lt;function&lt;void()&gt;&gt; mOnResetBAICallbacks.
	/// </summary>
	public static int ResetCallbacks { get; private set; } = 192;
	public static int TagVector { get; private set; } = 216;

	/// <summary>
	///     Takes a value's position as measured on this server. The name is the one the item file
	///     writes the value under, so what was measured and what is applied cannot drift apart.
	/// </summary>
	public static bool UseMeasured(string field, int at)
	{
		switch (field)
		{
			case "version": ParseVersion = at; return true;
			case "textureAtlas": TextureAtlas = at; return true;
			case "iconFrameCount": IconFrameCount = at; return true;
			case "animatesInToolbar": AnimatesInToolbar = at; return true;
			case "mirroredArt": MirroredArt = at; return true;
			case "useAnimation": UseAnimation = at; return true;
			case "icon": Icon = at; return true;
			case "icon2": SecondIcon = at; return true;
			case "maxStackSize": MaxStackSize = at; return true;
			case "id": Id = at; return true;
			case "translationKey": TranslationKey = at; return true;
			case "maxDurability": MaxDurability = at; return true;
			case "flags": Flags = at; return true;
			case "useDuration": UseDuration = at; return true;
			case "creativeCategory": CreativeCategory = at; return true;
			case "creativeGroup": CreativeGroup = at; return true;
			case "furnaceFuel": FurnaceFuel = at; return true;
			case "smeltingExperience": SmeltingExperience = at; return true;
			case "hiddenInCommands": HiddenInCommands = at; return true;
			case "rarity": Rarity = at; return true;
			case "mineBlockType": MineBlockType = at; return true;
			default: return false;
		}
	}
}
