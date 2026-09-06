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
using System.Globalization;
using System.IO;
using System.Text.Json;
using MiNET.Blocks;

namespace MiNET.AgentClient
{
	/// <summary>
	///     One colour per runtime id, taken from the map colours BDS carries for every block
	///     (MiNET.BdsExtract/Data/blocks.json). Biome-tinted blocks carry theirs on a
	///     BlockMapColorComponent with a tint method, which resolves here to one fixed tint each.
	///     Blocks with no colour anywhere get a by-name fallback, and beyond that a hash colour, so
	///     nothing is invisible by accident. Alpha zero means the ray passes through.
	/// </summary>
	public class BlockColors
	{
		public readonly struct Rgba
		{
			public readonly byte R, G, B, A;

			public Rgba(byte r, byte g, byte b, byte a)
			{
				R = r;
				G = g;
				B = b;
				A = a;
			}

			public static readonly Rgba Clear = new Rgba(0, 0, 0, 0);
		}

		/// <summary>
		///     How a block fills its cell, for the picture only: a cube, or one of four stand-ins
		///     for the blocks that are not cubes. The extraction says which blocks are opaque full
		///     cubes; the stand-in for the rest is chosen by name family, since the extraction
		///     carries no usable collision boxes for them.
		/// </summary>
		public enum BlockShape : byte
		{
			Cube,
			/// <summary>Plants, torches, saplings, candles: a short post in the middle of the cell.</summary>
			Post,
			/// <summary>Slabs and the like: the bottom half.</summary>
			Slab,
			/// <summary>Fences, walls, panes, bars, doors, signs: a full-height thin post.</summary>
			Thin,
			/// <summary>Carpets, snow layers, pressure plates, rails, wire: a thin sheet on the floor.</summary>
			Flat
		}

		private readonly Dictionary<string, Rgba> _byName = new Dictionary<string, Rgba>(StringComparer.Ordinal);
		private readonly Dictionary<string, bool> _fullByName = new Dictionary<string, bool>(StringComparer.Ordinal);
		private Rgba[] _byRuntimeId;
		private BlockShape[] _shapeByRuntimeId;

		public int NamedCount => _byName.Count;

		/// <summary>One shape per runtime id, resolved with the colour table.</summary>
		public BlockShape[] Shapes
		{
			get
			{
				_ = Table;
				return _shapeByRuntimeId;
			}
		}

		public BlockShape Shape(int runtimeId)
		{
			BlockShape[] shapes = Shapes;
			return runtimeId >= 0 && runtimeId < shapes.Length ? shapes[runtimeId] : BlockShape.Cube;
		}

		private static BlockShape ShapeOf(string name, bool opaqueFullBlock)
		{
			if (opaqueFullBlock) return BlockShape.Cube;
			string bare = name.StartsWith("minecraft:", StringComparison.Ordinal) ? name.Substring(10) : name;

			if (bare.EndsWith("_slab", StringComparison.Ordinal) || bare.EndsWith("_trapdoor", StringComparison.Ordinal)) return BlockShape.Slab;

			if (bare.EndsWith("_fence", StringComparison.Ordinal) || bare.EndsWith("_fence_gate", StringComparison.Ordinal) || bare.EndsWith("_wall", StringComparison.Ordinal)
				|| bare.EndsWith("_pane", StringComparison.Ordinal) || bare.EndsWith("_bars", StringComparison.Ordinal) || bare.EndsWith("_chain", StringComparison.Ordinal) || bare == "chain"
				|| bare.EndsWith("_door", StringComparison.Ordinal) || bare.EndsWith("_sign", StringComparison.Ordinal) || bare.EndsWith("_banner", StringComparison.Ordinal)
				|| bare == "ladder" || bare == "iron_bars")
				return BlockShape.Thin;

			if (bare.EndsWith("_carpet", StringComparison.Ordinal) || bare == "snow_layer" || bare.EndsWith("_pressure_plate", StringComparison.Ordinal)
				|| bare.EndsWith("_rail", StringComparison.Ordinal) || bare == "rail" || bare == "redstone_wire" || bare == "trip_wire" || bare == "moss_carpet" || bare == "lily_pad")
				return BlockShape.Flat;

			// Every plant, fungus, crop, sapling, flower and small fixture the map draws as a dot.
			if (bare.EndsWith("_sapling", StringComparison.Ordinal) || bare.EndsWith("_grass", StringComparison.Ordinal) || bare.EndsWith("_fern", StringComparison.Ordinal) || bare == "fern"
				|| bare.EndsWith("_flower", StringComparison.Ordinal) || bare.EndsWith("_tulip", StringComparison.Ordinal) || bare.EndsWith("_orchid", StringComparison.Ordinal)
				|| bare.EndsWith("_petals", StringComparison.Ordinal) || bare.EndsWith("_bush", StringComparison.Ordinal) || bare.EndsWith("_mushroom", StringComparison.Ordinal)
				|| bare.EndsWith("_fungus", StringComparison.Ordinal) || bare.EndsWith("_roots", StringComparison.Ordinal) || bare.EndsWith("_sprouts", StringComparison.Ordinal)
				|| bare.EndsWith("_torch", StringComparison.Ordinal) || bare == "torch" || bare.EndsWith("_candle", StringComparison.Ordinal) || bare == "candle"
				|| bare.EndsWith("_candle_cake", StringComparison.Ordinal) || bare == "lantern" || bare == "soul_lantern" || bare == "end_rod" || bare == "lever"
				|| bare.EndsWith("_button", StringComparison.Ordinal) || bare == "flower_pot" || bare.EndsWith("_skull", StringComparison.Ordinal) || bare.EndsWith("_head", StringComparison.Ordinal)
				|| bare == "poppy" || bare == "dandelion" || bare == "allium" || bare == "azure_bluet" || bare == "cornflower" || bare == "lily_of_the_valley" || bare == "oxeye_daisy"
				|| bare == "wither_rose" || bare == "torchflower" || bare == "pitcher_plant" || bare == "sunflower" || bare == "lilac" || bare == "peony" || bare == "rose_bush"
				|| bare == "wheat" || bare == "carrots" || bare == "potatoes" || bare == "beetroot" || bare == "sugar_cane" || bare == "kelp" || bare == "seagrass" || bare == "bamboo"
				|| bare == "cactus" || bare == "sweet_berry_bush" || bare == "dead_bush" || bare == "cobweb" || bare == "vine" || bare == "glow_lichen" || bare == "pointed_dripstone"
				|| bare == "sea_pickle" || bare == "brewing_stand" || bare == "cauldron" || bare == "hopper" || bare == "bell" || bare == "anvil" || bare == "grindstone")
				return BlockShape.Post;

			// Stairs, leaves, glass, water, ice, fence gates and everything else that fills its
			// cell well enough to read as one.
			return BlockShape.Cube;
		}

		/// <summary>One colour per runtime id, resolved over the whole palette up front so the ray loop reads an array.</summary>
		public Rgba[] Table => _byRuntimeId ??= BuildTable();

		private Rgba[] BuildTable()
		{
			int count = BlockFactory.BlockPalette.Count;
			var table = new Rgba[count];
			var shapes = new BlockShape[count];
			for (int runtimeId = 0; runtimeId < count; runtimeId++)
			{
				string name = BlockFactory.GetBlockName(runtimeId);
				Rgba color;
				if (name == null) color = HashColor(runtimeId.ToString(CultureInfo.InvariantCulture));
				else if (!_byName.TryGetValue(name, out color)) color = Fallback(name);
				table[runtimeId] = color;
				shapes[runtimeId] = name == null ? BlockShape.Cube : ShapeOf(name, _fullByName.TryGetValue(name, out bool full) && full);
			}
			_shapeByRuntimeId = shapes;
			return table;
		}

		public static BlockColors Load(string blocksJsonPath)
		{
			var colors = new BlockColors();
			using FileStream stream = File.OpenRead(blocksJsonPath);
			using JsonDocument document = JsonDocument.Parse(stream);

			foreach (JsonElement block in document.RootElement.GetProperty("blocks").EnumerateArray())
			{
				string name = block.GetProperty("name").GetString();
				if (name == null) continue;

				Rgba color = ParseColor(block.GetProperty("mapColor").GetString());

				// The component form wins: it is what the tinted blocks carry, and the block-level
				// value on those is transparent black.
				if (block.TryGetProperty("components", out JsonElement components))
				{
					foreach (JsonElement component in components.EnumerateArray())
					{
						if (component.ValueKind != JsonValueKind.Object) continue;
						if (!component.TryGetProperty("name", out JsonElement componentName) || componentName.GetString() != "BlockMapColorComponent") continue;

						Rgba baseColor = ParseColor(component.GetProperty("mapColor").GetString());
						string tint = component.TryGetProperty("tintMethod", out JsonElement tintMethod) && tintMethod.TryGetProperty("name", out JsonElement tintName)
							? tintName.GetString()
							: "None";
						color = ApplyTint(baseColor, tint);
					}
				}

				// Every leaves block carries the same biome-tinted component colour, so the
				// species table in Fallback wins over it: two kinds of leaves must read as two tones.
				bool bark = (name.EndsWith("_log", StringComparison.Ordinal) || name.EndsWith("_wood", StringComparison.Ordinal))
					&& (name.Contains("birch", StringComparison.Ordinal) || name.Contains("dark_oak", StringComparison.Ordinal) || name.Contains("spruce", StringComparison.Ordinal));
				if (color.A == 0 || bark || name.EndsWith("_leaves", StringComparison.Ordinal)) color = Fallback(name);
				colors._byName[name] = color;
				colors._fullByName[name] = block.TryGetProperty("isOpaqueFullBlock", out JsonElement full) && full.ValueKind == JsonValueKind.True;
			}

			return colors;
		}

		public Rgba Get(int runtimeId)
		{
			Rgba[] table = Table;
			return runtimeId >= 0 && runtimeId < table.Length ? table[runtimeId] : Rgba.Clear;
		}

		private static Rgba ParseColor(string hex)
		{
			if (string.IsNullOrEmpty(hex) || hex[0] != '#' || hex.Length != 9) return Rgba.Clear;
			byte r = byte.Parse(hex.AsSpan(1, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.AsSpan(3, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.AsSpan(5, 2), NumberStyles.HexNumber);
			byte a = byte.Parse(hex.AsSpan(7, 2), NumberStyles.HexNumber);
			return new Rgba(r, g, b, a);
		}

		// One fixed tint per method: a plains biome, since the picture is for reading shape and
		// material, not climate.
		private static Rgba ApplyTint(Rgba color, string tint)
		{
			switch (tint)
			{
				case "Grass": return Multiply(color, 0x91, 0xBD, 0x59, 255);
				case "Water": return Multiply(color, 0x44, 0xAF, 0xF5, 160);
				case "None": return color;
				default: return Multiply(color, 0x77, 0xAB, 0x2F, 255); // every foliage variant
			}
		}

		private static Rgba Multiply(Rgba color, int r, int g, int b, byte alpha)
		{
			return new Rgba((byte) (color.R * r / 255), (byte) (color.G * g / 255), (byte) (color.B * b / 255), alpha);
		}

		/// <summary>
		///     The blocks BDS gives no map colour: things a map draws nothing for. The ray must still
		///     stop on the solid ones, so they get a colour by family, and the see-through ones stay
		///     clear.
		/// </summary>
		private static Rgba Fallback(string name)
		{
			string bare = name.StartsWith("minecraft:", StringComparison.Ordinal) ? name.Substring(10) : name;

			if (bare == "air" || bare == "barrier" || bare == "structure_void" || bare == "invisible_bedrock" || bare == "client_request_placeholder_block" || bare.StartsWith("light_block", StringComparison.Ordinal))
				return Rgba.Clear;

			// Bark colours by species: the map colour is the plank colour, which is not what a
			// trunk looks like, and birch bark in particular is white with dark flecks.
			if ((bare.StartsWith("birch_", StringComparison.Ordinal) || bare.StartsWith("stripped_birch", StringComparison.Ordinal)) && (bare.EndsWith("_log", StringComparison.Ordinal) || bare.EndsWith("_wood", StringComparison.Ordinal))) return new Rgba(0xE4, 0xE0, 0xD6, 255);
			if (bare.StartsWith("dark_oak_", StringComparison.Ordinal) && (bare.EndsWith("_log", StringComparison.Ordinal) || bare.EndsWith("_wood", StringComparison.Ordinal))) return new Rgba(0x3E, 0x2A, 0x16, 255);
			if (bare.StartsWith("spruce_", StringComparison.Ordinal) && (bare.EndsWith("_log", StringComparison.Ordinal) || bare.EndsWith("_wood", StringComparison.Ordinal))) return new Rgba(0x4A, 0x33, 0x1E, 255);
			if (bare.EndsWith("_log", StringComparison.Ordinal)) return new Rgba(0x6B, 0x50, 0x30, 255);

			// The extraction gives every leaves block a transparent map colour (biome tinted in
			// the client), so the species get their own greens here; a build that shades a
			// canopy with two kinds of leaves has to read as two tones in the picture.
			if (bare.EndsWith("_leaves", StringComparison.Ordinal))
			{
				switch (bare)
				{
					case "oak_leaves": return new Rgba(0x5E, 0x96, 0x30, 255);
					case "dark_oak_leaves": return new Rgba(0x35, 0x5A, 0x1E, 255);
					case "spruce_leaves": return new Rgba(0x4A, 0x74, 0x4C, 255);
					case "birch_leaves": return new Rgba(0x80, 0xA7, 0x55, 255);
					case "jungle_leaves": return new Rgba(0x46, 0x9C, 0x2A, 255);
					case "acacia_leaves": return new Rgba(0x74, 0xA2, 0x32, 255);
					case "mangrove_leaves": return new Rgba(0x3C, 0x78, 0x2C, 255);
					case "pale_oak_leaves": return new Rgba(0x78, 0x8A, 0x6C, 255);
					case "cherry_leaves": return new Rgba(0xE6, 0xA8, 0xD0, 255);
					case "azalea_leaves": return new Rgba(0x5A, 0x90, 0x3A, 255);
					case "azalea_leaves_flowered": return new Rgba(0x70, 0x98, 0x50, 255);
					default: return new Rgba(0x55, 0x8C, 0x30, 255);
				}
			}
			if (bare.Contains("glass", StringComparison.Ordinal)) return new Rgba(0xC8, 0xE8, 0xF0, 90);
			if (bare.EndsWith("_bars", StringComparison.Ordinal) || bare.EndsWith("_chain", StringComparison.Ordinal)) return new Rgba(0x9A, 0x9A, 0x9A, 255);
			if (bare.EndsWith("_button", StringComparison.Ordinal) || bare.EndsWith("_rail", StringComparison.Ordinal) || bare == "rail" || bare == "lever" || bare == "ladder" || bare == "trip_wire" || bare == "tripwire_hook")
				return new Rgba(0x8A, 0x7A, 0x66, 255);
			if (bare.Contains("torch", StringComparison.Ordinal) || bare == "end_rod") return new Rgba(0xF0, 0xC8, 0x50, 255);
			if (bare.Contains("redstone", StringComparison.Ordinal) || bare.Contains("comparator", StringComparison.Ordinal) || bare.Contains("repeater", StringComparison.Ordinal))
				return new Rgba(0xB0, 0x30, 0x30, 255);
			if (bare.EndsWith("_candle_cake", StringComparison.Ordinal) || bare == "cake" || bare == "candle_cake") return new Rgba(0xF2, 0xE2, 0xD2, 255);
			if (bare.EndsWith("_skull", StringComparison.Ordinal) || bare.EndsWith("_head", StringComparison.Ordinal)) return new Rgba(0xD8, 0xD8, 0xC0, 255);
			if (bare.StartsWith("element_", StringComparison.Ordinal)) return new Rgba(0x60, 0x60, 0x70, 255);
			if (bare == "portal") return new Rgba(0x80, 0x20, 0xC0, 200);
			if (bare == "unknown") return new Rgba(0xFF, 0x00, 0xFF, 255);

			return HashColor(bare);
		}

		private static Rgba HashColor(string key)
		{
			uint hash = 2166136261;
			foreach (char c in key) hash = (hash ^ c) * 16777619;
			return new Rgba((byte) (0x60 + (hash & 0x7F)), (byte) (0x60 + ((hash >> 8) & 0x7F)), (byte) (0x60 + ((hash >> 16) & 0x7F)), 255);
		}
	}
}