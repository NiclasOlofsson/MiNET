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

using System;
using System.Collections.Generic;
using System.Linq;
using fNbt;

namespace MiNET.Blocks
{
	/// <summary>
	///     What StartGame declares about one data-driven block: the tree the client reads a block it
	///     does not already know from. The members are the wire's fields in C# spelling; the wire's
	///     own names live only in <see cref="BlockDefinitionWriter" />, so a name is written in one
	///     place.
	///     A null member is a field the wire leaves out, which is a different statement from a field
	///     holding a default: the server writes requires_correct_tool_for_drops only when it is true,
	///     and permutations only when the block has one.
	/// </summary>
	public sealed record BlockDefinition(
		IReadOnlyList<string> Tags,
		BlockMenuCategory MenuCategory,
		VanillaBlockData VanillaBlockData,
		BlockComponents Components,
		IReadOnlyList<BlockPermutation> Permutations,
		int MolangVersion = 13);

	/// <summary>Where the block sits in the creative menu, and whether commands offer it.</summary>
	public sealed record BlockMenuCategory(string Category, string Group, bool HiddenInCommands);

	/// <summary>
	///     The part of the definition the vanilla block itself states rather than a component:
	///     its numeric id, its material, and the archetype that decides how the client places it.
	/// </summary>
	public sealed record VanillaBlockData(
		int BlockId,
		string Material,
		BlockArchetype Archetype,
		bool RequiresCorrectToolForDrops,
		float? Translucency,
		bool CanDampenVibrations,
		bool CanOccludeVibrations);

	/// <summary>
	///     One archetype per shape family. The type name is the wire's and belongs to the record
	///     because it names the record; every other member is read off the block.
	/// </summary>
	public abstract record BlockArchetype(string Type);

	public sealed record StairArchetype(string BaseBlock) : BlockArchetype("stair_block");

	public sealed record SlabArchetype(string DoubleSlabBlock, bool IsDouble) : BlockArchetype("slab_block");

	public sealed record BushArchetype() : BlockArchetype("bush_block");

	public sealed record WallFoliageArchetype(int SizeCount, bool IsBonemealable, float Bounciness, float FallDamageMultiplier)
		: BlockArchetype("wall_foliage_block");

	/// <summary>A condition over the block's states and the components that hold while it is true.</summary>
	public sealed record BlockPermutation(string Condition, BlockComponents Components);

	/// <summary>
	///     One optional member per component the wire carries. Null means the component is absent.
	///     <see cref="Replaceable" /> is present-or-absent only: the wire writes it as an empty
	///     compound, so true means "the block is replaceable" and there is nothing inside to carry.
	/// </summary>
	public sealed record BlockComponents(
		float? DestructibleByMining = null,
		DestructionParticles DestructionParticles = null,
		IReadOnlyList<LiquidDetectionRule> LiquidDetection = null,
		RedstoneConductivity RedstoneConductivity = null,
		string SupportShape = null,
		CollisionBox CollisionBox = null,
		SelectionBox SelectionBox = null,
		BlockGeometry Geometry = null,
		MaterialInstances MaterialInstances = null,
		Transformation Transformation = null,
		ConnectionRule ConnectionRule = null,
		string PrecipitationBehavior = null,
		bool? Replaceable = null);

	public sealed record DestructionParticles(int ParticleCount, string Texture, string TintMethod);

	public sealed record LiquidDetectionRule(bool CanContainLiquid, string LiquidType, string OnLiquidTouches, byte StopsLiquidFromDirection, bool UseLiquidClipping);

	public sealed record RedstoneConductivity(bool RedstoneConductor, bool AllowsWireToStepDown);

	public sealed record CollisionBox(bool Enabled, IReadOnlyList<Aabb> Boxes);

	public sealed record SelectionBox(bool Enabled, Vec3 Origin, Vec3 Size);

	/// <summary>
	///     The model the client draws the block with. BoneVisibility is the map the wire always
	///     writes, empty for every block that locks its uvs with a flag instead of naming bones.
	/// </summary>
	public sealed record BlockGeometry(
		string Identifier,
		string Culling,
		string CullingLayer,
		string CullingShape,
		bool NeedsLegacyTopRotation,
		bool UseBlockTypeLightAbsorption,
		bool IgnoreGeometryForIsSolid,
		bool IsV1Fullblock,
		bool UvLock,
		IReadOnlyDictionary<string, bool> BoneVisibility = null);

	public sealed record MaterialInstances(IReadOnlyDictionary<string, MaterialInstance> Materials, IReadOnlyDictionary<string, string> Mappings = null);

	/// <summary>
	///     One face's material. PackedBools is the five render flags the server packs into one byte
	///     and the client unpacks; nothing here splits them, because the wire does not.
	/// </summary>
	public sealed record MaterialInstance(float AmbientOcclusion, byte PackedBools, string RenderMethod, string Texture, string TintMethod);

	public sealed record Transformation(Vec3 Translation, Vec3I Rotation, Vec3 RotationPivot, Vec3 Scale, Vec3 ScalePivot, bool HasJsonVersionBeforeValidation);

	public sealed record ConnectionRule(string AcceptsConnectionsFrom, IReadOnlyList<string> EnabledDirections);

	public readonly record struct Vec3(float X, float Y, float Z);

	public readonly record struct Vec3I(int X, int Y, int Z);

	public readonly record struct Aabb(Vec3 Min, Vec3 Max);

	/// <summary>
	///     Turns a <see cref="BlockDefinition" /> into the network NBT StartGame carries for it.
	///     Every compound goes out in ordinal name order, which is what BDS writes and what the
	///     entry bytes are compared against.
	/// </summary>
	public static class BlockDefinitionWriter
	{
		public static NbtCompound Write(BlockDefinition definition)
		{
			var root = new NbtCompound("");

			if (definition.Tags is {Count: > 0})
			{
				var tags = new NbtList("blockTags", NbtTagType.String);
				foreach (string tag in definition.Tags) tags.Add(new NbtString(tag));
				root.Add(tags);
			}

			root.Add(WriteComponents("components", definition.Components));
			root.Add(new NbtCompound("menu_category")
			{
				new NbtString("category", definition.MenuCategory.Category),
				new NbtString("group", definition.MenuCategory.Group),
				new NbtByte("is_hidden_in_commands", definition.MenuCategory.HiddenInCommands ? (byte) 1 : (byte) 0)
			});
			root.Add(new NbtInt("molangVersion", definition.MolangVersion));

			if (definition.Permutations is {Count: > 0})
			{
				var permutations = new NbtList("permutations", NbtTagType.Compound);
				foreach (BlockPermutation permutation in definition.Permutations)
				{
					permutations.Add(new NbtCompound
					{
						WriteComponents("components", permutation.Components),
						new NbtString("condition", permutation.Condition)
					});
				}

				root.Add(permutations);
			}

			root.Add(WriteVanillaBlockData(definition.VanillaBlockData));

			return Sorted(root);
		}

		/// <summary>Network NBT: little endian, varint lengths, unnamed root.</summary>
		public static byte[] ToNetworkBytes(NbtCompound root)
		{
			var copy = (NbtCompound) root.Clone();
			copy.Name = "";
			return new NbtFile(copy) {BigEndian = false, UseVarInt = true}.SaveToBuffer(NbtCompression.None);
		}

		private static NbtCompound WriteComponents(string name, BlockComponents components)
		{
			var compound = new NbtCompound(name);
			if (components == null) return compound;

			if (components.CollisionBox != null)
			{
				// An empty list carries element type End on the wire, which is what BDS writes for a
				// block whose collision is switched off.
				IReadOnlyList<Aabb> shapes = components.CollisionBox.Boxes ?? [];
				var boxes = new NbtList("boxes", shapes.Count == 0 ? NbtTagType.End : NbtTagType.Compound);
				foreach (Aabb box in shapes)
				{
					boxes.Add(new NbtCompound
					{
						new NbtFloat("maxX", box.Max.X),
						new NbtFloat("maxY", box.Max.Y),
						new NbtFloat("maxZ", box.Max.Z),
						new NbtFloat("minX", box.Min.X),
						new NbtFloat("minY", box.Min.Y),
						new NbtFloat("minZ", box.Min.Z)
					});
				}

				compound.Add(new NbtCompound("minecraft:collision_box")
				{
					boxes,
					new NbtByte("enabled", components.CollisionBox.Enabled ? (byte) 1 : (byte) 0)
				});
			}

			if (components.ConnectionRule != null)
			{
				var directions = new NbtList("enabled_directions", NbtTagType.String);
				foreach (string direction in components.ConnectionRule.EnabledDirections) directions.Add(new NbtString(direction));
				compound.Add(new NbtCompound("minecraft:connection_rule")
				{
					new NbtString("accepts_connections_from", components.ConnectionRule.AcceptsConnectionsFrom),
					directions
				});
			}

			if (components.DestructibleByMining.HasValue)
			{
				compound.Add(new NbtCompound("minecraft:destructible_by_mining") {new NbtFloat("value", components.DestructibleByMining.Value)});
			}

			if (components.DestructionParticles != null)
			{
				compound.Add(new NbtCompound("minecraft:destruction_particles")
				{
					new NbtInt("particle_count", components.DestructionParticles.ParticleCount),
					new NbtString("texture", components.DestructionParticles.Texture),
					new NbtString("tint_method", components.DestructionParticles.TintMethod)
				});
			}

			if (components.Geometry != null)
			{
				var bones = new NbtCompound("bone_visibility");
				if (components.Geometry.BoneVisibility != null)
				{
					foreach ((string bone, bool visible) in components.Geometry.BoneVisibility) bones.Add(new NbtByte(bone, visible ? (byte) 1 : (byte) 0));
				}

				compound.Add(new NbtCompound("minecraft:geometry")
				{
					bones,
					new NbtString("culling", components.Geometry.Culling),
					new NbtString("culling_layer", components.Geometry.CullingLayer),
					new NbtString("culling_shape", components.Geometry.CullingShape),
					new NbtString("identifier", components.Geometry.Identifier),
					new NbtByte("ignoreGeometryForIsSolid", components.Geometry.IgnoreGeometryForIsSolid ? (byte) 1 : (byte) 0),
					new NbtByte("isV1Fullblock", components.Geometry.IsV1Fullblock ? (byte) 1 : (byte) 0),
					new NbtByte("needsLegacyTopRotation", components.Geometry.NeedsLegacyTopRotation ? (byte) 1 : (byte) 0),
					new NbtByte("useBlockTypeLightAbsorption", components.Geometry.UseBlockTypeLightAbsorption ? (byte) 1 : (byte) 0),
					new NbtByte("uv_lock", components.Geometry.UvLock ? (byte) 1 : (byte) 0)
				});
			}

			if (components.LiquidDetection != null)
			{
				var rules = new NbtList("detectionRules", NbtTagType.Compound);
				foreach (LiquidDetectionRule rule in components.LiquidDetection)
				{
					rules.Add(new NbtCompound
					{
						new NbtByte("canContainLiquid", rule.CanContainLiquid ? (byte) 1 : (byte) 0),
						new NbtString("liquidType", rule.LiquidType),
						new NbtString("onLiquidTouches", rule.OnLiquidTouches),
						new NbtByte("stopsLiquidFromDirection", rule.StopsLiquidFromDirection),
						new NbtByte("use_liquid_clipping", rule.UseLiquidClipping ? (byte) 1 : (byte) 0)
					});
				}

				compound.Add(new NbtCompound("minecraft:liquid_detection") {rules});
			}

			if (components.MaterialInstances != null)
			{
				var mappings = new NbtCompound("mappings");
				if (components.MaterialInstances.Mappings != null)
				{
					foreach ((string face, string instance) in components.MaterialInstances.Mappings) mappings.Add(new NbtString(face, instance));
				}

				var materials = new NbtCompound("materials");
				foreach ((string face, MaterialInstance instance) in components.MaterialInstances.Materials)
				{
					materials.Add(new NbtCompound(face)
					{
						new NbtFloat("ambient_occlusion", instance.AmbientOcclusion),
						new NbtByte("packed_bools", instance.PackedBools),
						new NbtString("render_method", instance.RenderMethod),
						new NbtString("texture", instance.Texture),
						new NbtString("tint_method", instance.TintMethod)
					});
				}

				compound.Add(new NbtCompound("minecraft:material_instances") {mappings, materials});
			}

			if (components.PrecipitationBehavior != null)
			{
				compound.Add(new NbtCompound("minecraft:precipitation_interactions") {new NbtString("precipitation_behavior", components.PrecipitationBehavior)});
			}

			if (components.RedstoneConductivity != null)
			{
				compound.Add(new NbtCompound("minecraft:redstone_conductivity")
				{
					new NbtByte("allowsWireToStepDown", components.RedstoneConductivity.AllowsWireToStepDown ? (byte) 1 : (byte) 0),
					new NbtByte("redstoneConductor", components.RedstoneConductivity.RedstoneConductor ? (byte) 1 : (byte) 0)
				});
			}

			if (components.Replaceable == true) compound.Add(new NbtCompound("minecraft:replaceable"));

			if (components.SelectionBox != null)
			{
				compound.Add(new NbtCompound("minecraft:selection_box")
				{
					new NbtByte("enabled", components.SelectionBox.Enabled ? (byte) 1 : (byte) 0),
					Floats("origin", components.SelectionBox.Origin),
					Floats("size", components.SelectionBox.Size)
				});
			}

			if (components.SupportShape != null)
			{
				compound.Add(new NbtCompound("minecraft:support") {new NbtString("shape", components.SupportShape)});
			}

			if (components.Transformation != null)
			{
				Transformation t = components.Transformation;
				compound.Add(new NbtCompound("minecraft:transformation")
				{
					new NbtInt("RX", t.Rotation.X),
					new NbtFloat("RXP", t.RotationPivot.X),
					new NbtInt("RY", t.Rotation.Y),
					new NbtFloat("RYP", t.RotationPivot.Y),
					new NbtInt("RZ", t.Rotation.Z),
					new NbtFloat("RZP", t.RotationPivot.Z),
					new NbtFloat("SX", t.Scale.X),
					new NbtFloat("SXP", t.ScalePivot.X),
					new NbtFloat("SY", t.Scale.Y),
					new NbtFloat("SYP", t.ScalePivot.Y),
					new NbtFloat("SZ", t.Scale.Z),
					new NbtFloat("SZP", t.ScalePivot.Z),
					new NbtFloat("TX", t.Translation.X),
					new NbtFloat("TY", t.Translation.Y),
					new NbtFloat("TZ", t.Translation.Z),
					new NbtByte("hasJsonVersionBeforeValidation", t.HasJsonVersionBeforeValidation ? (byte) 1 : (byte) 0)
				});
			}

			return compound;
		}

		private static NbtCompound WriteVanillaBlockData(VanillaBlockData data)
		{
			var archetype = new NbtCompound("block_archetype") {new NbtString("type", data.Archetype.Type)};
			switch (data.Archetype)
			{
				case StairArchetype stair:
					archetype.Add(new NbtString("base_block", stair.BaseBlock));
					break;
				case SlabArchetype slab:
					archetype.Add(new NbtString("double_slab_block", slab.DoubleSlabBlock));
					archetype.Add(new NbtByte("is_double", slab.IsDouble ? (byte) 1 : (byte) 0));
					break;
				case WallFoliageArchetype foliage:
					archetype.Add(new NbtFloat("bounciness", foliage.Bounciness));
					archetype.Add(new NbtFloat("fall_damage_multiplier", foliage.FallDamageMultiplier));
					archetype.Add(new NbtByte("is_bonemealable", foliage.IsBonemealable ? (byte) 1 : (byte) 0));
					archetype.Add(new NbtInt("size_count", foliage.SizeCount));
					break;
			}

			var compound = new NbtCompound("vanilla_block_data")
			{
				archetype,
				new NbtInt("block_id", data.BlockId),
				new NbtString("material", data.Material)
			};

			if (data.CanDampenVibrations) compound.Add(new NbtByte("can_dampen_vibrations", 1));
			if (data.CanOccludeVibrations) compound.Add(new NbtByte("can_occlude_vibrations", 1));
			if (data.RequiresCorrectToolForDrops) compound.Add(new NbtByte("requires_correct_tool_for_drops", 1));
			if (data.Translucency.HasValue) compound.Add(new NbtFloat("translucency", data.Translucency.Value));

			return compound;
		}

		private static NbtList Floats(string name, Vec3 value)
		{
			return new NbtList(name, NbtTagType.Float) {new NbtFloat(value.X), new NbtFloat(value.Y), new NbtFloat(value.Z)};
		}

		/// <summary>
		///     Rebuilds every compound in ordinal name order. It rebuilds rather than reorders,
		///     because a compound's children live in a dictionary and removing them all and adding
		///     them back reuses the freed slots.
		/// </summary>
		private static NbtCompound Sorted(NbtCompound compound)
		{
			var result = new NbtCompound(compound.Name);
			foreach (NbtTag child in compound.OrderBy(c => c.Name, StringComparer.Ordinal)) result.Add(SortedTag(child));
			return result;
		}

		private static NbtTag SortedTag(NbtTag tag)
		{
			switch (tag)
			{
				case NbtCompound compound:
					return Sorted(compound);
				case NbtList list:
				{
					var result = new NbtList(list.Name, list.ListType);
					foreach (NbtTag element in list) result.Add(SortedTag(element));
					return result;
				}
				default:
					return (NbtTag) tag.Clone();
			}
		}
	}
}
