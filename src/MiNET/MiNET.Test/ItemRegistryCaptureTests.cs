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
using System.IO;
using System.Linq;
using fNbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Test
{
	/// <summary>
	///     What the server declares about items has to be what vanilla declares, because the client
	///     interprets every item id, stack and creative slot through it. MiNET.BlockGen proves the
	///     trees it emits against these same frames; this proves the compiled result, which is the
	///     part a generator cannot check: the emitted construction code, the registry's one-time
	///     serialization, and the typed JSON the creative catalog is stored as.
	/// </summary>
	[TestClass]
	public class ItemRegistryCaptureTests
	{
		private static string CaptureDirectory => Path.Combine(AppContext.BaseDirectory, "Data", "registry");

		[TestMethod]
		public void Item_registry_declares_what_bds_declares()
		{
			var packet = McpeItemComponent.CreateObject();
			packet.Decode(File.ReadAllBytes(Path.Combine(CaptureDirectory, "item_registry-1.26.50.26.bin")).AsMemory());

			ItemRegistry registry = ItemFactory.ItemRegistry;
			Assert.AreEqual(packet.entries.Count, registry.Count, "the registry declares a different number of items than BDS does");

			var differences = new List<string>();
			foreach (ItemComponent expected in packet.entries)
			{
				if (!registry.TryGetByName(expected.Name, out ItemRegistryEntry actual))
				{
					differences.Add($"{expected.Name}: BDS declares it, the registry does not");
					continue;
				}

				if (actual.NetworkId != expected.RuntimeId) differences.Add($"{expected.Name}: network id {actual.NetworkId}, BDS declares {expected.RuntimeId}");
				if (actual.ComponentBased != expected.ComponentBased) differences.Add($"{expected.Name}: component_based {actual.ComponentBased}, BDS declares {expected.ComponentBased}");
				if (actual.Version != expected.Version) differences.Add($"{expected.Name}: version {actual.Version}, BDS declares {expected.Version}");

				byte[] expectedNbt = NetworkNbt((NbtCompound) expected.Nbt.NbtFile.RootTag);
				byte[] actualNbt = actual.ComponentNbt ?? NetworkNbt(new NbtCompound(""));
				if (!actualNbt.SequenceEqual(expectedNbt))
				{
					differences.Add($"{expected.Name}: component tree is {actualNbt.Length} bytes, BDS declares {expectedNbt.Length}");
				}
			}

			Assert.AreEqual(0, differences.Count, string.Join(Environment.NewLine, differences.Take(20)));
		}

		[TestMethod]
		public void Creative_catalog_declares_what_bds_declares()
		{
			var packet = McpeCreativeContent.CreateObject();
			packet.Decode(File.ReadAllBytes(Path.Combine(CaptureDirectory, "creative_content-1.26.50.26.bin")).AsMemory());

			CreativeGroupData catalog = InventoryUtils.CreativeGroups.Value;
			Assert.AreEqual(packet.groups.Count, catalog.Groups.Count, "the catalog declares a different number of groups than BDS does");
			Assert.AreEqual(packet.entries.Count, catalog.Entries.Count, "the catalog declares a different number of entries than BDS does");

			var differences = new List<string>();
			for (int i = 0; i < catalog.Groups.Count; i++)
			{
				CreativeGroupDef actual = catalog.Groups[i];
				CreativeGroupInfoPayload expected = packet.groups[i];

				if (actual.Category != (int) expected.creativeCategory) differences.Add($"group {i} {actual.Name}: category {actual.Category}, BDS declares {(int) expected.creativeCategory}");
				if (actual.Name != expected.name) differences.Add($"group {i}: name '{actual.Name}', BDS declares '{expected.name}'");
				CompareStack($"group {i} {actual.Name} icon", actual.IconNetworkId, actual.IconMetadata, actual.IconRuntimeId, actual.IconNbt, expected.groupIconItem, differences);
			}

			for (int i = 0; i < catalog.Entries.Count; i++)
			{
				CreativeEntryDef actual = catalog.Entries[i];
				CreativeItemEntryPayload expected = packet.entries[i];

				if (actual.GroupIndex != expected.groupIndex) differences.Add($"entry {i}: group {actual.GroupIndex}, BDS declares {expected.groupIndex}");
				CompareStack($"entry {i}", actual.NetworkId, actual.Metadata, actual.RuntimeId, actual.Nbt, expected.itemInstance, differences);
			}

			Assert.AreEqual(0, differences.Count, string.Join(Environment.NewLine, differences.Take(20)));
		}

		private static void CompareStack(string what, int networkId, short metadata, int runtimeId, Newtonsoft.Json.Linq.JObject nbt, Item expected, List<string> differences)
		{
			// An empty stack decodes to air, which carries the registry's own air id rather than the
			// zero the wire uses for "no item".
			int expectedNetworkId = expected == null || expected.IsAir ? 0 : expected.NetworkId;
			if (networkId != expectedNetworkId) differences.Add($"{what}: network id {networkId}, BDS declares {expectedNetworkId}");
			if (expectedNetworkId != 0)
			{
				if (metadata != expected.NetworkMetadata) differences.Add($"{what}: metadata {metadata}, BDS declares {expected.NetworkMetadata}");
				if (runtimeId != expected.RuntimeId) differences.Add($"{what}: block runtime id {runtimeId}, BDS declares {expected.RuntimeId}");
			}

			NbtCompound actualExtra = nbt == null ? null : TypedNbtJson.ReadCompound(nbt);
			NbtCompound expectedExtra = expectedNetworkId == 0 ? null : expected.ExtraData;
			if (actualExtra == null && expectedExtra == null) return;
			if (actualExtra == null || expectedExtra == null || !FixedNbt(actualExtra).SequenceEqual(FixedNbt(expectedExtra)))
			{
				differences.Add($"{what}: user data {(actualExtra == null ? "absent" : "present")}, BDS declares it {(expectedExtra == null ? "absent" : "present")}");
			}
		}

		private static byte[] NetworkNbt(NbtCompound compound)
		{
			var root = (NbtCompound) compound.Clone();
			root.Name = "";
			return new NbtFile(root) {BigEndian = false, UseVarInt = true}.SaveToBuffer(NbtCompression.None);
		}

		/// <summary>Item extra data is fixed little endian NBT, which is how it goes on the wire.</summary>
		private static byte[] FixedNbt(NbtCompound compound)
		{
			var root = (NbtCompound) compound.Clone();
			root.Name = "";
			return new NbtFile(root) {BigEndian = false, UseVarInt = false}.SaveToBuffer(NbtCompression.None);
		}
	}
}
