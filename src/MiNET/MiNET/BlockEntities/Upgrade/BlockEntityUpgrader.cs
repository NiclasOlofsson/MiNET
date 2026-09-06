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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Blocks.Upgrade;
using MiNET.Entities;
using MiNET.Items.Upgrade;
using MiNET.Utils;

namespace MiNET.BlockEntities.Upgrade
{
	/// <summary>
	///     Rewrites the item stacks inside a block entity as it comes off disk, so what a chest holds is
	///     current before anything looks at it.
	///     <para>
	///     This has to happen at load, not when a container is opened. The block entity NBT read from
	///     the world is what rides along inline with the chunk, so a stack left in its 2017 form goes
	///     out to a current client exactly as it was written, whatever the server would have made of it
	///     later.
	///     </para>
	/// </summary>
	public static class BlockEntityUpgrader
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockEntityUpgrader));

		private static readonly ConcurrentDictionary<string, byte> Reported = new();

		/// <summary>
		///     Walks the whole compound rather than a list of known container types: a stack can sit in
		///     <c>Items</c>, in <c>Item</c> alone (item frames, jukeboxes), in numbered slots (campfires
		///     write <c>Item1</c> through <c>Item4</c>), or inside another stack's <c>tag</c>, as with a
		///     shulker box in a chest.
		/// </summary>
		public static void Upgrade(NbtCompound blockEntity)
		{
			if (blockEntity == null) return;

			UpgradeTags(blockEntity);
			UpgradeFields(blockEntity);
		}

		/// <summary>
		///     What the tile itself stores, as opposed to what is inside it. Mojang has moved fields
		///     around over the years and a current client reads the current shape only: a sign whose
		///     text is still a single blob draws blank, and a spawner with only the old numeric id
		///     spawns nothing.
		/// </summary>
		private static void UpgradeFields(NbtCompound tile)
		{
			switch (tile["id"]?.StringValue)
			{
				case "Sign":
					UpgradeSign(tile);
					break;
				case "MobSpawner":
					UpgradeSpawner(tile);
					break;
				case "Furnace":
				case "BlastFurnace":
				case "Smoker":
					UpgradeFurnace(tile);
					break;
			}
		}

		/// <summary>
		///     Signs gained a second side in 1.19.80. The text, its colour and its glow moved into a
		///     FrontText compound, with BackText beside it, and everything before that is one face
		///     stored flat: a Text blob since 1.2, or Text1 through Text4 before it.
		/// </summary>
		private static void UpgradeSign(NbtCompound sign)
		{
			if (sign["FrontText"] is NbtCompound) return;

			string text = sign["Text"]?.StringValue;
			if (text == null)
			{
				var lines = new List<string>();
				for (int line = 1; line <= 4; line++) lines.Add(sign[$"Text{line}"]?.StringValue ?? string.Empty);

				while (lines.Count > 0 && lines[^1].Length == 0) lines.RemoveAt(lines.Count - 1);
				text = string.Join("\n", lines);
			}

			int color = sign["SignTextColor"]?.IntValue ?? unchecked((int) 0xff000000);

			// The glow flag only means anything once the lighting bug was resolved, which is what the
			// flag beside it records. Reading it unconditionally makes old signs glow.
			bool glowing = sign["TextIgnoreLegacyBugResolved"]?.ByteValue == 1 && sign["IgnoreLighting"]?.ByteValue == 1;

			foreach (string field in new[] {"Text", "Text1", "Text2", "Text3", "Text4", "SignTextColor", "IgnoreLighting", "TextIgnoreLegacyBugResolved", "TextOwner"}) sign.Remove(field);

			sign.Add(SignFace("FrontText", text, color, glowing));
			sign.Add(SignFace("BackText", string.Empty, color, false));
			// A sign written between 1.19.80 and the waxing update can already carry IsWaxed without FrontText.
			if (sign["IsWaxed"] == null) sign.Add(new NbtByte("IsWaxed", 0));
		}

		private static NbtCompound SignFace(string name, string text, int color, bool glowing)
		{
			return new NbtCompound(name)
			{
				new NbtString("Text", text),
				new NbtString("TextOwner", string.Empty),
				new NbtInt("SignTextColor", color),
				new NbtByte("IgnoreLighting", (byte) (glowing ? 1 : 0)),
				new NbtByte("PersistFormatting", 1),
				new NbtByte("HideGlowOutline", 0)
			};
		}

		/// <summary>
		///     A spawner used to name its mob with a numeric type id. The values are not always the
		///     bare id: worlds from 1.2 and 1.6 hold it packed into a larger int, with the id in the
		///     low byte, so 0x110b22 is 0x22, the skeleton. Every value in the regression corpus
		///     decodes that way and matches the mobs the newer worlds name outright.
		/// </summary>
		private static void UpgradeSpawner(NbtCompound spawner)
		{
			if (spawner["EntityIdentifier"] is NbtString) return;
			if (spawner["EntityId"] is not NbtInt legacy) return;

			string identifier = IdentifierOf(legacy.Value) ?? IdentifierOf(legacy.Value & 0xff);
			if (identifier == null)
			{
				Report(spawner, $"spawner entity id {legacy.Value} names no known mob");
				return;
			}

			spawner.Remove("EntityId");
			spawner.Add(new NbtString("EntityIdentifier", identifier));
		}

		private static string IdentifierOf(int legacyId)
		{
			string identifier = ((EntityType) legacyId).ToStringId();
			return identifier == ":" ? null : identifier;
		}

		/// <summary>
		///     The experience a furnace is holding outgrew its short in 1.16.100 and became
		///     StoredXPInt. A client reading the current shape finds nothing where the old field is.
		/// </summary>
		private static void UpgradeFurnace(NbtCompound furnace)
		{
			if (furnace["StoredXPInt"] != null || furnace["StoredXP"] is not NbtShort stored) return;

			furnace.Remove("StoredXP");
			furnace.Add(new NbtInt("StoredXPInt", stored.Value));
		}

		private static void UpgradeTags(NbtTag tag)
		{
			switch (tag)
			{
				case NbtCompound compound:
					if (IsItemStack(compound)) UpgradeStack(compound);
					else if (IsBlockState(compound)) UpgradeBlockState(compound);

					foreach (NbtTag child in new List<NbtTag>(compound)) UpgradeTags(child);
					break;

				case NbtList list:
					foreach (NbtTag child in new List<NbtTag>(list)) UpgradeTags(child);
					break;
			}
		}

		/// <summary>
		///     A stack always has a count and an identity, and the identity is one of the three shapes
		///     Bedrock has used. Nothing else in a block entity carries that pair, so this is a safe
		///     test to apply to every compound in the tree.
		/// </summary>
		private static bool IsItemStack(NbtCompound compound)
		{
			if (compound["Count"] == null) return false;

			return compound["Name"] is NbtString || compound["id"] is NbtShort || compound["id"] is NbtString;
		}

		/// <summary>
		///     A stored blockstate that is not an item: a flower pot keeps what is planted in it as
		///     <c>PlantBlock</c>, and a piston arm keeps the blocks it is pushing. Same shape as a
		///     palette entry, so it needs the same chain or the pot comes back holding a block the
		///     current palette does not have.
		/// </summary>
		private static bool IsBlockState(NbtCompound compound)
		{
			if (compound["name"] is not NbtString) return false;

			// Before 1.13 the state was a numeric val rather than a states compound, which is how a
			// flower pot from that era stores what is planted in it.
			return compound["states"] is NbtCompound || compound["val"] != null;
		}

		private static void UpgradeBlockState(NbtCompound block)
		{
			if (!BlockDataUpgrader.TryUpgrade(block, out string name, out List<IBlockState> states)) return;

			block.Remove("name");
			block.Remove("states");
			block.Remove("val");
			block.Remove("version");

			block.Add(new NbtString("name", name));
			block.Add(StatesOf(states));
			block.Add(new NbtInt("version", (int) BlockDataUpgrader.CurrentVersion));
		}

		private static NbtCompound StatesOf(List<IBlockState> states)
		{
			var compound = new NbtCompound("states");
			foreach (IBlockState state in states)
			{
				switch (state)
				{
					case BlockStateByte value:
						compound.Add(new NbtByte(value.Name, value.Value));
						break;
					case BlockStateInt value:
						compound.Add(new NbtInt(value.Name, value.Value));
						break;
					case BlockStateString value:
						compound.Add(new NbtString(value.Name, value.Value));
						break;
				}
			}

			return compound;
		}

		private static void UpgradeStack(NbtCompound stack)
		{
			short damage = stack["Damage"]?.ShortValue ?? 0;

			// A block item written between 1.9 and the flattening carries its variant in the Block
			// compound rather than in Damage, so that compound, not the name, says which block this
			// is. Upgrading it first also leaves the stack's own Block tag current.
			string fromBlock = stack["Block"] is NbtCompound stored0 ? UpgradeBlockTag(stack, stored0) : null;

			string name;
			int meta;
			if (stack["Name"] is NbtString stored)
			{
				(name, meta) = ItemDataUpgrader.Upgrade(stored.StringValue, damage);
			}
			else if (stack["id"] is NbtShort legacyId)
			{
				// Air was never supposed to be saved as a stack, but old versions wrote it anyway.
				if (legacyId.Value == 0)
				{
					Replace(stack, "minecraft:air", 0);
					return;
				}

				(name, meta) = ItemDataUpgrader.Upgrade(legacyId.Value, damage);
			}
			else if (stack["id"] is NbtString javaId)
			{
				// The Java save format. The string ids mostly line up, so it is worth a try.
				(name, meta) = ItemDataUpgrader.Upgrade(javaId.StringValue, damage);
			}
			else
			{
				return;
			}

			if (name == null)
			{
				Report(stack, "no current item for this stack");
				return;
			}

			if (fromBlock != null && ItemDataUpgrader.IsKnownItem(fromBlock))
			{
				name = fromBlock;
				meta = 0;
			}

			Replace(stack, name, meta);
		}

		/// <summary>
		///     The block a block item stands for, stored the same way a palette entry is, so it goes
		///     through the same chain. Answers with the upgraded block name.
		/// </summary>
		private static string UpgradeBlockTag(NbtCompound stack, NbtCompound block)
		{
			if (!BlockDataUpgrader.TryUpgrade(block, out string name, out List<IBlockState> states))
			{
				stack.Remove("Block");
				return null;
			}

			var upgraded = new NbtCompound("Block")
			{
				new NbtString("name", name),
				StatesOf(states),
				new NbtInt("version", (int) BlockDataUpgrader.CurrentVersion)
			};

			stack.Remove("Block");
			stack.Add(upgraded);

			return name;
		}

		private static void Replace(NbtCompound stack, string name, int meta)
		{
			stack.Remove("id");
			stack.Remove("Name");
			stack.Remove("Damage");

			stack.Add(new NbtString("Name", name));
			stack.Add(new NbtShort("Damage", (short) meta));
		}

		private static void Report(NbtCompound stack, string reason)
		{
			string identity = stack["Name"]?.StringValue ?? stack["id"]?.StringValue ?? "<no identity>";
			if (Reported.TryAdd($"{identity}|{reason}", 0)) Log.Warn($"Item {identity} in a block entity: {reason}");
		}
	}
}
