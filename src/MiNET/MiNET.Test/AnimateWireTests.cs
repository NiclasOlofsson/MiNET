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
	/// <summary>
	///     Animate is relayed to every other player each time someone swings, so its tail is on
	///     the wire constantly. Generated from Mojang's AnimatePacket schema at 2207 with two
	///     measured corrections: a real 1.26.60.21 client's swing is
	///     <c>2c 01 02 00000000 00</c>, action, runtime id, data float and ONE swing-source byte,
	///     no presence bytes and no hand, and that client disconnected with a level 2 violation
	///     on 0x2C when a server packet ended there. So swing source is a plain byte, and hand is a
	///     trailing default: always written, accepted as absent when reading.
	/// </summary>
	[TestClass]
	public class AnimateWireTests
	{
		[TestMethod]
		public void Action_values_follow_the_schema_not_the_declaration_order()
		{
			Assert.AreEqual(0, (int) McpeAnimate.AnimatePacketPayloadAction.Noaction);
			Assert.AreEqual(1, (int) McpeAnimate.AnimatePacketPayloadAction.Swing);
			Assert.AreEqual(3, (int) McpeAnimate.AnimatePacketPayloadAction.Wakeup);
			Assert.AreEqual(4, (int) McpeAnimate.AnimatePacketPayloadAction.Criticalhit);
			Assert.AreEqual(5, (int) McpeAnimate.AnimatePacketPayloadAction.Magiccriticalhit);
		}

		[TestMethod]
		public void Tail_is_float_swing_source_byte_and_hand_byte()
		{
			var packet = McpeAnimate.CreateObject();
			packet.actionId = McpeAnimate.AnimatePacketPayloadAction.Swing;
			packet.runtimeEntityId = 5;
			packet.data = 0f;
			packet.swingSource = McpeAnimate.ActorSwingSource.Mine;
			packet.hand = McpeAnimate.HandSlot.Offhand;

			string hex = Convert.ToHexString(packet.Encode()).ToLowerInvariant();
			Assert.IsTrue(hex.EndsWith("01" + "05" + "00000000" + "02" + "01"), hex);
		}

		[TestMethod]
		public void Hand_is_written_even_when_it_is_the_default()
		{
			var packet = McpeAnimate.CreateObject();
			packet.actionId = McpeAnimate.AnimatePacketPayloadAction.Criticalhit;
			packet.runtimeEntityId = 5;

			string hex = Convert.ToHexString(packet.Encode()).ToLowerInvariant();
			Assert.IsTrue(hex.EndsWith("04" + "05" + "00000000" + "00" + "00"), hex);
		}

		[TestMethod]
		public void A_real_client_swing_without_the_hand_decodes()
		{
			// The captured client frame: action 1, runtime id 2, data 0, swing source None, end.
			var reference = McpeAnimate.CreateObject();
			reference.actionId = McpeAnimate.AnimatePacketPayloadAction.Swing;
			reference.runtimeEntityId = 2;
			byte[] full = reference.Encode();
			byte[] clientFrame = full[..^1];

			var decoded = McpeAnimate.CreateObject();
			decoded.Decode(clientFrame);

			Assert.AreEqual(McpeAnimate.AnimatePacketPayloadAction.Swing, decoded.actionId);
			Assert.AreEqual(2L, decoded.runtimeEntityId);
			Assert.AreEqual(McpeAnimate.ActorSwingSource.None, decoded.swingSource);
			Assert.AreEqual(McpeAnimate.HandSlot.Mainhand, decoded.hand);
		}

		[TestMethod]
		public void Round_trip_keeps_every_field()
		{
			var packet = McpeAnimate.CreateObject();
			packet.actionId = McpeAnimate.AnimatePacketPayloadAction.Magiccriticalhit;
			packet.runtimeEntityId = 123456;
			packet.data = 1.5f;
			packet.swingSource = McpeAnimate.ActorSwingSource.Attack;
			packet.hand = McpeAnimate.HandSlot.Offhand;

			var decoded = McpeAnimate.CreateObject();
			decoded.Decode(packet.Encode());

			Assert.AreEqual(McpeAnimate.AnimatePacketPayloadAction.Magiccriticalhit, decoded.actionId);
			Assert.AreEqual(123456L, decoded.runtimeEntityId);
			Assert.AreEqual(1.5f, decoded.data);
			Assert.AreEqual(McpeAnimate.ActorSwingSource.Attack, decoded.swingSource);
			Assert.AreEqual(McpeAnimate.HandSlot.Offhand, decoded.hand);
		}
	}
}