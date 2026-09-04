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
///     Where an item keeps each of its values, from the reference, on the same terms as the block
///     and state classes: every offset is stated from the object's start.
///     <para>
///         What the item side reads from is the name, because the name is the one place in an item
///         this tool can find without being told: it verifies against its own hash. So the offsets
///         come back out of here stated from the name instead, which is the class's own offset less
///         how far into the object the name was measured to sit.
///     </para>
/// </summary>
public static class ItemLayout
{
	/// <summary>The item class, as the tree it is.</summary>
	public static ClassTree Items => BlockMembers.Tree(BlockMembers.Source.Items);

	/// <summary>The class, as the reference states it.</summary>
	public static int Size => BlockMembers.Root(BlockMembers.Source.Items).Size;

	/// <summary>The object's own start, which is where its method table sits.</summary>
	public static int MethodTable => -ItemRegistry.NameInsideItem;

	/// <summary>Where a member sits, stated from the name.</summary>
	public static int At(string name) => Member(name).At - ItemRegistry.NameInsideItem;

	/// <summary>One member of the item class, by name.</summary>
	public static MemberNode Member(string name) =>
		Items.Roots.FirstOrDefault(n => n.Member.Name == name)
		?? throw new InvalidOperationException($"the item class has no member called {name}");

	public static int ParseVersion => At("version");
	public static int TextureAtlas => At("textureAtlas");
	public static int FrameCount => At("frameCount");
	public static int AnimatesInToolbar => At("animatesInToolbar");
	public static int MirroredArt => At("mirroredArt");
	public static int UseAnimation => At("useAnimation");
	public static int HoverTextColorFormat => At("hoverTextColorFormat");
	public static int IconName => At("iconName");
	public static int AtlasName => At("atlasName");
	public static int MaxStackSize => At("maxStackSize");
	public static int Id => At("id");
	public static int TranslationKey => At("translationKey");
	public static int BareName => At("bareName");
	public static int Namespace => At("namespace");
	public static int MaxDurability => At("maxDurability");
	public static int UseDuration => At("useDuration");
	public static int MinimumVersion => At("minimumVersion");
	public static int Block => At("block");
	public static int CreativeCategory => At("creativeCategory");
	public static int CraftingRemainingItem => At("craftingRemainingItem");
	public static int CreativeGroup => At("creativeGroup");
	public static int FurnaceFuel => At("furnaceFuel");
	public static int SmeltingExperience => At("smeltingExperience");
	public static int HiddenInCommands => At("hiddenInCommands");
	public static int Rarity => At("rarity");
	public static int MineBlockType => At("mineBlockType");
	public static int FoodComponent => At("foodComponent");
	public static int SeedComponent => At("seedComponent");
	public static int CameraComponent => At("cameraComponent");
	public static int ResetCallbacks => At("resetCallbacks");
	public static int TagVector => At("tags");
}
