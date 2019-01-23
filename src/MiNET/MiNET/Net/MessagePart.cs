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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;

namespace MiNET.Net
{
	public class MessagePart : Packet<MessagePart> // Replace this with stream
	{
		public MessagePartHeader Header { get; private set; }
		public Memory<byte> Buffer { get; set; }
		public byte ContainedMessageId { get; set; }

		public MessagePart()
		{
			Header = new MessagePartHeader();
		}

		public int GetPayloadSize()
		{
			return Buffer.Length;
		}

		public override void Reset()
		{
			base.Reset();
			Header.Reset();
			ContainedMessageId = 0;
			Buffer = null;
		}

		protected override void EncodePacket()
		{
			// DO NOT CALL base.EncodePackage();
			_buffer.Position = 0;

			Memory<byte> encodedMessage = Buffer;

			byte flags = (byte) Header.Reliability;
			Write((byte) ((flags << 5) | (Header.HasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (encodedMessage.Length * 8), true); // length

			if (Header.Reliability == Reliability.Reliable
				|| Header.Reliability == Reliability.ReliableOrdered
				|| Header.Reliability == Reliability.ReliableSequenced
				|| Header.Reliability == Reliability.ReliableWithAckReceipt
				|| Header.Reliability == Reliability.ReliableOrderedWithAckReceipt
			)
			{
				Write(Header.ReliableMessageNumber);
			}

			if (Header.Reliability == Reliability.UnreliableSequenced
				|| Header.Reliability == Reliability.ReliableOrdered
				|| Header.Reliability == Reliability.ReliableSequenced
				|| Header.Reliability == Reliability.ReliableOrderedWithAckReceipt
			)
			{
				Write(Header.OrderingIndex);
				Write(Header.OrderingChannel);
			}

			if (Header.HasSplit)
			{
				Write(Header.PartCount, true);
				Write(Header.PartId, true);
				Write(Header.PartIndex, true);
			}

			// Message body

			Write(encodedMessage);
		}
	}
}