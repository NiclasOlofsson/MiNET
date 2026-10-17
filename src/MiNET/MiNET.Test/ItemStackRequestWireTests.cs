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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;

namespace MiNET.Test
{
	[TestClass]
	public class ItemStackRequestWireTests
	{
		// Real bytes off a 1.26.40 client, taking a chest out of the creative menu. Every inventory
		// move a player makes arrives in this packet, so a decode that throws here takes the whole
		// request with it and the item silently does not move. These are the client's bytes, not
		// ours, which is the point: a self round-trip only ever exercises values we already emit,
		// and the bug this test was written for lived in a field we never write. Each action is its
		// position in the schema's option list as a varint, then its action type as a byte. 2225
		// inserted a reserved craft action ahead of the two deprecated ones, so the results action
		// below is re-tagged from the captured 11 13 to 12 14; every other byte is the client's.
		private const string CreativePickOfAChest =
			"9301" +                                 // packet id 147, unsigned varint
			"01" +                                   // one request
			"a901" +                                 // client request id -85
			"03" +                                   // three actions
			"0c" + "0e" + "e10a" + "01" +            // CraftCreative: creative entry 1377, one craft
			"12" + "14" + "01" +                     // CraftResultsDeprecated, one result
			"01" + "01" +                            // descriptor variant 1, ItemName
			"0f" + "6d696e6563726166743a6368657374" + // "minecraft:chest"
			"04" +                                   // aux value 2
			"0100" +                                 // stack size 1
			"f871" +                                 // block runtime id 14584
			"0a" + "00000000000000000000" +          // user data buffer, ten bytes
			"01" +                                   // one craft
			"00" + "00" + "40" +                     // Take, 64 of them
			"3c" + "00" + "32" + "abffffff" +        // from created output slot 50, net id -85
			"3b" + "00" + "00" + "00000000" +        // to the cursor slot 0, no net id yet
			"00" +                                   // no strings to filter
			"ffffffff";                              // text processing origin, none

		// The tag after each variant is a byte, not a string. Reading it as a string ate the length
		// prefix of the name that followed, and the next read took 0x6d off the front of
		// "minecraft:chest" as a length, which threw and dropped the request.
		[TestMethod]
		public void CreativePick_DecodesTheItemNameDescriptor()
		{
			var packet = McpeItemStackRequest.CreateObject();
			packet.Decode(Convert.FromHexString(CreativePickOfAChest));

			Assert.AreEqual(1, packet.requests.Count);
			ItemStackRequest request = packet.requests[0];
			Assert.AreEqual(-85, request.clientRequestId);
			Assert.AreEqual(3, request.actions.Count);

			var creative = request.actions[0] as ItemStackRequestCraftCreativeAction;
			Assert.IsNotNull(creative, "the first action names the creative entry that was picked");
			Assert.AreEqual(1377u, creative.creativeItemNetId);
			Assert.AreEqual(1, creative.numberOfRequestedCrafts);

			var results = request.actions[1] as ItemStackRequestCraftResultsDeprecatedAction;
			Assert.IsNotNull(results, "the second action is the client's claim about what it produced");
			Assert.AreEqual(1, results.craftResults.Count);

			ItemStackRequestNetworkItemInstanceDescriptor descriptor = results.craftResults[0];
			var name = descriptor.itemDescriptor as ItemNameDescriptor;
			Assert.IsNotNull(name, "a creative pick names the item, it does not send a tag or a molang expression");
			Assert.AreEqual("minecraft:chest", name.fullName);
			Assert.AreEqual(2, name.auxValue);
			Assert.AreEqual(1, descriptor.stackSize);
			Assert.AreEqual(14584u, descriptor.blockRuntimeId);
			Assert.AreEqual(10, descriptor.userDataBuffer.Length);

			var take = request.actions[2] as ItemStackRequestTakeAction;
			Assert.IsNotNull(take, "the third action moves the produced stack out of the output slot");
			Assert.AreEqual(64, take.amount);
			Assert.AreEqual(FullContainerName.ContainerEnumName.Createdoutputcontainer, take.source.fullContainerName.containerName);
			Assert.AreEqual(50, take.source.slot);

			// The stack net id is a fixed li32. Read as a signed varint it takes one byte too many
			// and swallows the next container name, which put the destination one slot earlier in
			// the enum: AnvilInputContainer (0) instead of CursorContainer (59). The server then
			// told the client its chest was in an anvil and the client rolled the pick back.
			Assert.AreEqual(-85, take.source.netIdVariant, "the source net id is the client request id, li32");
			Assert.AreEqual(FullContainerName.ContainerEnumName.Cursorcontainer, take.destination.fullContainerName.containerName);
			Assert.AreEqual(0, take.destination.slot);

			// Only the correct width leaves the tail aligned. Off by one byte this reads -65536, and
			// McpeItemStackRequest warns that two bytes are left over.
			Assert.AreEqual(-1, (int) request.stringstofilterorigin, "the request ends clean, with nothing left to read");
		}

		// The byte after each action's tag is the action type, not the tag: the action enum skips
		// 7 and 8 and the option list does not, so CraftCreative is 0c 0e. Writing the tag twice
		// still decodes, because the reader drops that byte, which is why only a re-encode sees it.
		[TestMethod]
		public void CreativePick_ReencodesToTheClientsOwnBytes()
		{
			var decoded = McpeItemStackRequest.CreateObject();
			decoded.Decode(Convert.FromHexString(CreativePickOfAChest));

			var packet = McpeItemStackRequest.CreateObject();
			packet.requests = decoded.requests;

			Assert.AreEqual(CreativePickOfAChest, Convert.ToHexString(packet.Encode()).ToLowerInvariant());
		}
	}
}
