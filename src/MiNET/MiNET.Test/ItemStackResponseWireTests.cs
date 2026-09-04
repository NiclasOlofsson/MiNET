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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Test
{
	[TestClass]
	public class ItemStackResponseWireTests
	{
		// The response is the server's ruling on what the client may do to its own inventory, so a
		// byte that moves here is a desync the client acts on rather than a rendering glitch. The
		// codec is being moved from a hand-written pair onto the Mojang schemas, and the two agree
		// only if this exact payload still encodes to these exact bytes. The payload is built to
		// cover every branch the wire has: an accepted request carrying containers and a refused one
		// carrying none, a slot whose stack has a network id and one whose stack has none, and a
		// second container so the per-container framing is exercised more than once.

		// An optional is one bool and then the value. It was two for a while: the generator wrapped
		// the field's own optionality around the struct's, so every one of these carried a spurious
		// always-true byte in front. Mojang's schema has Item Stack Net Id as an optional field whose
		// type is a struct with one required int, and Containers likewise, so one bool is the shape.
		// Corrected in 502bb755 alongside unrelated work, which is why this test is the only record
		// that it happened.
		//
		//   9401        packet id 0x94, as a varint
		//   02          two responses
		//   00          response 0: result Ok
		//   02          client request id 1, zigzag
		//   01          containers: present
		//   02          two containers
		//   1c 00       container Hotbar, no dynamic id
		//   02          two slots
		//   050520      requested slot 5, slot 5, amount 32
		//   0104        net id: present, id 2 zigzag
		//   0000 00     custom name empty, filtered empty, no durability correction
		//   060610      requested slot 6, slot 6, amount 16
		//   00          net id: absent
		//   0000 00
		//   3e 00       container CrafterLevelEntity, no dynamic id
		//   01          one slot
		//   000000      requested slot 0, slot 0, amount 0
		//   00          net id: absent
		//   0000 00
		//   01          response 1: result Error
		//   04          client request id 2, zigzag
		//   00          containers: absent
		private const string Expected =
			"9401" + "02" +
			"00" + "02" + "01" + "02" +
			"1c00" + "02" +
			"050520" + "0104" + "000000" +
			"060610" + "00" + "000000" +
			"3e00" + "01" +
			"000000" + "00" + "000000" +
			"01" + "04" + "00";

		[TestMethod]
		public void ItemStackResponse_EncodesToTheSameBytesItAlwaysHas()
		{
			var packet = McpeItemStackResponse.CreateObject();
			packet.responses = BuildResponses();

			byte[] encoded = packet.Encode();

			Assert.AreEqual(Expected, Convert.ToHexString(encoded).ToLowerInvariant());
		}

		private static List<ItemStackResponseInfo> BuildResponses()
		{
			return new List<ItemStackResponseInfo>
			{
				new ItemStackResponseInfo
				{
					result = ItemStackResponseInfo.Result.Success,
					clientRequestId = 1,
					containers = new List<ItemStackResponseContainerInfo>
					{
						new ItemStackResponseContainerInfo
						{
							fullContainerName = new FullContainerName {containerName = FullContainerName.ContainerEnumName.Hotbarcontainer},
							slots = new List<ItemStackResponseSlotInfo>
							{
								new ItemStackResponseSlotInfo {requestedSlot = 5, slot = 5, amount = 32, itemStackNetId = 2},
								new ItemStackResponseSlotInfo {requestedSlot = 6, slot = 6, amount = 16, itemStackNetId = null},
							}
						},
						new ItemStackResponseContainerInfo
						{
							fullContainerName = new FullContainerName {containerName = FullContainerName.ContainerEnumName.Crafterlevelentitycontainer},
							slots = new List<ItemStackResponseSlotInfo>
							{
								new ItemStackResponseSlotInfo {requestedSlot = 0, slot = 0, amount = 0, itemStackNetId = null},
							}
						}
					}
				},
				new ItemStackResponseInfo
				{
					result = ItemStackResponseInfo.Result.Error,
					clientRequestId = 2,
					containers = null
				}
			};
		}
	}
}
