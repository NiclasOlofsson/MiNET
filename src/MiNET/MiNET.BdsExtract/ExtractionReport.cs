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
///     Checks an extraction against itself, with no reference data of any kind.
///     This exists because the failure that matters is silent. If a field moved in a new server
///     build, the tool still reads a number, and that number is wrong without looking wrong. Each
///     check below is something the real palette cannot fail, so failing one means the read is
///     broken rather than the server being unusual.
/// </summary>
public sealed class ExtractionReport
{
	public int Slots { get; init; }
	public int NamedSlots { get; init; }
	public int DistinctNames { get; init; }
	public int NameRuns { get; init; }
	public int HashMismatches { get; init; }
	public int OrderViolations { get; init; }
	public int DuplicateNetworkIds { get; init; }
	public int LegacyIdConflicts { get; init; }

	/// <summary>
	///     Which id scheme the server was running. The server decides this with
	///     block-network-ids-are-hashes, and it changes what the network id field holds: the
	///     state's hash when hashed, otherwise just the palette position again. Worth reporting
	///     because nothing else about the extraction looks different, so a reader who assumed the
	///     wrong scheme would get plausible and useless numbers.
	/// </summary>
	public bool NetworkIdsAreHashes { get; init; }

	/// <summary>Every slot named, every name hashing correctly, states grouped, blocks in order.</summary>
	public bool Passed =>
		Slots > 0
		&& NamedSlots == Slots
		&& NameRuns == DistinctNames
		&& HashMismatches == 0
		&& OrderViolations == 0
		&& DuplicateNetworkIds == 0
		&& LegacyIdConflicts == 0;

	public static ExtractionReport Check(IReadOnlyList<PaletteEntry> palette)
	{
		int named = 0, runs = 0, hashMismatches = 0, orderViolations = 0, legacyConflicts = 0;
		var names = new HashSet<string>();
		var networkIds = new HashSet<uint>();
		var legacyByName = new Dictionary<string, int>();
		string previousName = null;
		ulong previousHash = 0;
		int duplicateNetworkIds = 0;
		bool everyIdIsItsIndex = true;

		foreach (var entry in palette)
		{
			if (entry.Name.Length > 0) named++;
			if (!networkIds.Add(entry.NetworkId)) duplicateNetworkIds++;
			if (entry.NetworkId != (uint) entry.Index) everyIdIsItsIndex = false;

			if (entry.Name != previousName)
			{
				runs++;
				names.Add(entry.Name);

				// The palette is ordered by the hash of the block name, ascending and unsigned.
				if (previousName is not null && entry.NameHash < previousHash) orderViolations++;
				previousHash = entry.NameHash;
				previousName = entry.Name;
			}

			// A name must hash to the value stored beside it, or it was not read correctly.
			if (entry.Name.Length > 0 && HashedString.Fnv1(entry.Name) != entry.NameHash) hashMismatches++;

			// Every state of a block reports the same numeric id as the block.
			if (entry.Name.Length > 0)
			{
				if (legacyByName.TryGetValue(entry.Name, out int seen))
				{
					if (seen != entry.LegacyId) legacyConflicts++;
				}
				else
				{
					legacyByName[entry.Name] = entry.LegacyId;
				}
			}
		}

		return new ExtractionReport
		{
			Slots = palette.Count,
			NamedSlots = named,
			DistinctNames = names.Count,
			NameRuns = runs,
			HashMismatches = hashMismatches,
			OrderViolations = orderViolations,
			DuplicateNetworkIds = duplicateNetworkIds,
			LegacyIdConflicts = legacyConflicts,
			NetworkIdsAreHashes = !everyIdIsItsIndex
		};
	}

	public void WriteTo(TextWriter output)
	{
		output.WriteLine($"  slots                 {Slots:N0}");
		output.WriteLine($"  named                 {NamedSlots:N0}{(NamedSlots == Slots ? "" : "   <- some slots unresolved")}");
		output.WriteLine($"  distinct blocks       {DistinctNames:N0}");
		output.WriteLine($"  contiguous runs       {NameRuns:N0}{(NameRuns == DistinctNames ? "" : "   <- a block's states are split")}");
		output.WriteLine($"  name hash mismatches  {HashMismatches:N0}");
		output.WriteLine($"  ordering violations   {OrderViolations:N0}");
		output.WriteLine($"  duplicate network ids {DuplicateNetworkIds:N0}");
		output.WriteLine($"  legacy id conflicts   {LegacyIdConflicts:N0}");
		output.WriteLine(NetworkIdsAreHashes
			? "  network ids           hashed, so networkId is the runtime id"
			: "  network ids           NOT hashed, so networkId only repeats index; the runtime"
			+ Environment.NewLine
			+ "                        id is the index. Restart with block-network-ids-are-hashes=true"
			+ Environment.NewLine
			+ "                        if you need the hashes.");
		output.WriteLine(Passed ? "  all checks passed" : "  CHECKS FAILED, the extraction is not trustworthy");
	}
}
