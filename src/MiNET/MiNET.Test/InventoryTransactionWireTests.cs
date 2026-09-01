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
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Test
{
	[TestClass]
	public class InventoryTransactionWireTests
	{
		// Every block a player places, breaks or uses arrives in this packet, so a byte out of
		// place here puts blocks somewhere other than where they were clicked. It is about to move
		// from a hand-written codec onto the Mojang schemas, and unlike the outbound packets this
		// one is read, not written, so both directions have to hold: the same transaction has to
		// encode to the same bytes, and those bytes have to decode back to the same values.

		[TestMethod]
		public void ItemUseTransaction_EncodesToTheSameBytesItAlwaysHas()
		{
			var packet = McpeInventoryTransaction.CreateObject();
			packet.legacyRequestId = 0;
			packet.transaction = BuildItemUse();

			byte[] encoded = packet.Encode();

			Assert.AreEqual(ExpectedItemUse, Convert.ToHexString(encoded).ToLowerInvariant());
		}

		[TestMethod]
		public void ItemUseTransaction_SurvivesARoundTrip()
		{
			var packet = McpeInventoryTransaction.CreateObject();
			packet.legacyRequestId = 0;
			packet.transaction = BuildItemUse();
			byte[] encoded = packet.Encode();

			var decoded = McpeInventoryTransaction.CreateObject();
			decoded.Decode(encoded);

			var source = (ItemUseInventoryTransaction) packet.transaction;
			var result = decoded.transaction as ItemUseInventoryTransaction;

			Assert.IsNotNull(result, "an item use transaction has to come back as one");
			Assert.AreEqual(packet.legacyRequestId, decoded.legacyRequestId);
			Assert.AreEqual(source.actionType, result.actionType);
			Assert.AreEqual(source.position, result.position);
			Assert.AreEqual(source.face, result.face);
			Assert.AreEqual(source.slot, result.slot);
			Assert.AreEqual(source.targetBlockId, result.targetBlockId);
			Assert.AreEqual(source.fromPosition, result.fromPosition);
			Assert.AreEqual(source.clickPosition, result.clickPosition);
		}

		// The 2168 capture from the hand-written codec, with two later corrections. A Hand enum
		// (uint8 varint, mainhand 0) arrived at 2192 between Face and the carried item, shifting
		// everything after it (Mojang's ItemUseInventoryTransaction schema, ordinal 6). And the two
		// always-true presence bytes that used to sit before the transaction type and the
		// transaction data are gone: the generator wrapped an optionality around fields that are not
		// optional in the schema. Corrected in 502bb755 alongside unrelated work.
		//
		//   1e            packet id 0x1e
		//   00            request id 0, zigzag
		//   00            no changed slots
		//   02            transaction type 2, item use
		//   00            no transaction records
		//   00            action 0, place
		//   01            trigger type 1, player input
		//   14 8201 27    position 10, 65, -20, three zigzag varints
		//   01            face 1
		//   06            slot 3, zigzag
		//   00            hand 0, mainhand (new at 2192)
		//   0000 0000 00 00 00 00      the carried item, air, eight bytes
		//   00002841 00008442 00009cc1 from position 10.5, 66.0, -19.5
		//   0000003f 0000803f 0000003f click position 0.5, 1.0, 0.5
		//   b015          block runtime id 2736
		//   00 00         client prediction, client cooldown state
		private const string ExpectedItemUse =
			"1e" + "00" + "00" + "02" + "00" +
			"00" + "01" + "14820127" + "01" + "06" + "00" +
			"0000000000000000" +
			"000028410000844200009cc1" +
			"0000003f0000803f0000003f" +
			"b015" + "0000";

		private static ItemUseInventoryTransaction BuildItemUse()
		{
			return new ItemUseInventoryTransaction
			{
				actionType = ItemUseInventoryTransaction.ItemUseActionType.Place,
				triggerType = ItemUseInventoryTransaction.ItemUseTriggerType.PlayerInput,
				position = new BlockCoordinates(10, 65, -20),
				face = 1,
				slot = 3,
				item = new ItemAir(),
				fromPosition = new Vector3(10.5f, 66.0f, -19.5f),
				clickPosition = new Vector3(0.5f, 1.0f, 0.5f),
				targetBlockId = 2736,
				actions = new List<InventoryAction>()
			};
		}
	}
}
