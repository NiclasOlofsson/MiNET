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

/// <summary>One block as the server holds it. These are properties of the block, not of its states.</summary>
public sealed class BlockProperties
{
	public string Name { get; init; }
	public ulong NameHash { get; init; }
	public int LegacyId { get; init; }

	public float Hardness { get; init; }
	public float ExplosionResistance { get; init; }
	public float Friction { get; init; }
	public float Thickness { get; init; }
	public float Translucency { get; init; }
	public int LightEmission { get; init; }
	public int LightDampening { get; init; }
	public int BurnOdds { get; init; }
	public int FlameOdds { get; init; }
	public bool IsSolid { get; init; }
	public bool CanContainLiquidSource { get; init; }
	public string LiquidReactionOnTouch { get; init; }
	public string TintMethod { get; init; }
	public string MapColor { get; init; }

	/// <summary>Address of the BlockLegacy this was read from. Only meaningful while the server lives.</summary>
	public ulong Address { get; init; }
}

/// <summary>
///     Finds every block in a running server and reads its properties.
///     The search has no list of expected names. It sweeps memory for the HashedString pattern, a
///     hash followed by text that hashes to it, which identifies a block name and nothing else.
///     Each block then points at a second object holding hardness and blast resistance, and both
///     are read at the offsets in <see cref="MemoryLayout" />.
/// </summary>
public static class BlockRegistry
{
	private const string Namespace = "minecraft:";

	public static List<BlockProperties> Read(BedrockProcess process)
	{
		var found = new Dictionary<ulong, BlockProperties>();
		var window = new byte[8 * 1024 * 1024];
		var heap = new byte[256];
		var material = new byte[MemoryLayout.MaterialSize];
		var word = new byte[8];

		foreach (var region in process.Regions)
		{
			// Overlap each window by one object so a block spanning a boundary is not missed.
			for (ulong at = region.Base; at < region.End; at += (ulong) window.Length - MemoryLayout.LegacyReach)
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (length < MemoryLayout.LegacyReach || !process.TryRead(at, window, length)) continue;

				for (int i = 0; i + MemoryLayout.LegacyReach <= length; i += 8)
				{
					string name = HashedString.ReadVerified(process, window, i, heap);
					if (name is null || !name.StartsWith(Namespace, StringComparison.Ordinal)) continue;

					// The HashedString sits partway into BlockLegacy, so step back to the object.
					ulong nameAddress = at + (ulong) i;
					if (nameAddress < MemoryLayout.NameInsideLegacy) continue;
					ulong legacy = nameAddress - MemoryLayout.NameInsideLegacy;
					var block = ReadBlock(process, legacy, name, window, i, material, word);
					if (block is null) continue;

					// Several copies of a name can exist; keep the one that reads as a real block.
					if (!found.TryGetValue(block.NameHash, out var existing) || Score(block) > Score(existing))
					{
						found[block.NameHash] = block;
					}
				}
			}
		}
		return found.Values.OrderBy(b => b.Name, StringComparer.Ordinal).ToList();
	}

	private static BlockProperties ReadBlock(BedrockProcess process, ulong legacy, string name,
		byte[] window, int at, byte[] material, byte[] word)
	{
		ulong materialAddress = process.ReadUInt64(legacy + MemoryLayout.NameInsideLegacy + MemoryLayout.MaterialPointer, word);
		if (materialAddress < 0x10000 || !process.IsMapped(materialAddress)) return null;
		if (!process.TryRead(materialAddress, material, MemoryLayout.MaterialSize)) return null;

		int nameAt = at; // the HashedString, and every BlockLegacy field is relative to it
		var color = new float[4];
		for (int c = 0; c < 4; c++)
		{
			color[c] = BitConverter.ToSingle(window, nameAt + MemoryLayout.MapColor + (c * 4));
		}

		return new BlockProperties
		{
			Name = name,
			NameHash = BitConverter.ToUInt64(window, nameAt),
			LegacyId = BitConverter.ToUInt16(window, nameAt + MemoryLayout.LegacyId),
			Thickness = BitConverter.ToSingle(window, nameAt + MemoryLayout.Thickness),
			Translucency = BitConverter.ToSingle(window, nameAt + MemoryLayout.Translucency),
			TintMethod = MemoryLayout.Describe(MemoryLayout.TintMethods,
				window[nameAt + MemoryLayout.TintMethod]),
			MapColor = ToHex(color),
			Hardness = BitConverter.ToSingle(material, MemoryLayout.MaterialHardness),
			ExplosionResistance = BitConverter.ToSingle(material, MemoryLayout.MaterialExplosionResistance),
			Friction = BitConverter.ToSingle(material, MemoryLayout.MaterialFriction),
			LightEmission = material[MemoryLayout.MaterialLightEmission],
			LightDampening = material[MemoryLayout.MaterialLightDampening],
			BurnOdds = material[MemoryLayout.MaterialBurnOdds],
			FlameOdds = material[MemoryLayout.MaterialFlameOdds],
			IsSolid = material[MemoryLayout.MaterialIsSolid] != 0,
			CanContainLiquidSource = material[MemoryLayout.MaterialCanContainLiquid] != 0,
			LiquidReactionOnTouch = MemoryLayout.Describe(MemoryLayout.LiquidReactions,
				material[MemoryLayout.MaterialLiquidReaction]),
			Address = legacy
		};
	}

	/// <summary>How much a candidate reads like a real block, used only to choose between copies of one name.</summary>
	private static int Score(BlockProperties block)
	{
		int score = 0;
		if (block.Friction is > 0f and <= 1f) score += 2;
		if (block.LegacyId < 4096) score += 1;
		if (float.IsFinite(block.Hardness) && block.Hardness is >= -1f and < 1e7f) score += 2;
		if (float.IsFinite(block.ExplosionResistance) && block.ExplosionResistance is >= 0f and < 1e8f) score += 2;
		return score;
	}

	private static string ToHex(float[] rgba)
	{
		static int Channel(float value) => Math.Clamp((int) Math.Round(value * 255f), 0, 255);
		return $"#{Channel(rgba[0]):X2}{Channel(rgba[1]):X2}{Channel(rgba[2]):X2}{Channel(rgba[3]):X2}";
	}
}
