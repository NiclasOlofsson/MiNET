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

using fNbt;
using MiNET.Items;

namespace TestPlugin
{
	/// <summary>
	///     A component-based item the client has no built-in definition for. Everything it shows
	///     (icon, name, the attribute lines in the tooltip) comes from the registry entry below.
	///     Test item: a custom id wearing the vanilla iron axe icon.
	/// </summary>
	public class ItemTestAxe : Item
	{
		public const string Id = "minet:test_axe";

		/// <summary>Above every vanilla id (883 in the 1.26.50 extract), below the short ceiling.</summary>
		public const short NetworkIdValue = 900;

		public ItemTestAxe() : base(Id)
		{
		}

		/// <summary>
		///     Adds the registry entry. The item_registry packet is built from
		///     <see cref="ItemFactory.ItemRegistry" /> on every join, so registering once at plugin
		///     enable is enough. Component names and value types follow the BDS item schemas
		///     (docs/json_schemas/server/item); the item_properties block mirrors what BDS sends
		///     for its own data-driven items (the spears).
		/// </summary>
		public static void Register()
		{
			if (ItemFactory.ItemRegistry.Contains(Id)) return;

			var components = new NbtCompound("components")
			{
				new NbtCompound("item_properties")
				{
					new NbtByte("allow_off_hand", 0),
					new NbtByte("can_destroy_in_creative", 1),
					new NbtInt("creative_category", 3),
					new NbtString("creative_group", ""),
					new NbtInt("damage", 0),
					new NbtString("enchantable_slot", "axe"),
					new NbtInt("enchantable_value", 14),
					new NbtByte("foil", 0),
					new NbtInt("frame_count", 1),
					new NbtByte("hand_equipped", 1),
					new NbtByte("hidden_in_commands", 0),
					new NbtByte("liquid_clipped", 0),
					new NbtInt("max_stack_size", 1),
					new NbtCompound("minecraft:icon")
					{
						new NbtCompound("textures")
						{
							// The client's item_texture.json has no "iron_axe" key: the axes are one
							// "axe" entry holding an array of tier textures, iron at index 2.
							new NbtString("default", "axe")
						}
					},
					new NbtFloat("mining_speed", 1f),
					new NbtByte("should_despawn", 1),
					new NbtByte("stacked_by_data", 0),
					new NbtInt("use_animation", 0),
					new NbtInt("use_duration", 0)
				},
				new NbtList("item_tags", NbtTagType.String)
				{
					new NbtString("minecraft:iron_tier"),
					new NbtString("minecraft:is_axe")
				},
				new NbtCompound("minecraft:digger")
				{
					new NbtByte("use_efficiency", 1),
					new NbtList("destroy_speeds", NbtTagType.Compound)
					{
						new NbtCompound()
						{
							new NbtCompound("block")
							{
								new NbtString("name", ""),
								new NbtString("tags", "q.any_tag('wood')"),
								new NbtCompound("states")
							},
							new NbtInt("speed", 8)
						}
					}
				},
				// The three registry-level text knobs. They show on a stack that carries no
				// display NBT of its own; display.Name on the stack overrides the name.
				new NbtCompound("minecraft:display_name")
				{
					new NbtString("value", "Registry Name")
				},
				new NbtCompound("minecraft:hover_text_color")
				{
					new NbtString("value", "minecoin_gold")
				},
				new NbtCompound("minecraft:rarity")
				{
					new NbtString("value", "epic")
				},
				new NbtCompound("minecraft:durability")
				{
					new NbtCompound("damage_chance")
					{
						new NbtInt("max", 100),
						new NbtInt("min", 0)
					},
					new NbtInt("max_durability", 250)
				},
				new NbtCompound("minecraft:enchantable")
				{
					new NbtString("slot", "axe"),
					new NbtByte("value", 14)
				},
				new NbtCompound("minecraft:hand_equipped")
				{
					new NbtByte("value", 1)
				},
				new NbtCompound("minecraft:max_stack_size")
				{
					new NbtByte("value", 1)
				}
			};

			ItemFactory.ItemRegistry.Add(Id, NetworkIdValue, true, 1, components);
		}
	}
}
