using System;

namespace MiNET.Net
{
	public class MessagePart : Package<MessagePart> // Replace this with stream
	{
		public MessagePartHeader Header { get; private set; }
		public byte[] Buffer { get; set; }
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

		protected override void EncodePackage()
		{
			// DO NOT CALL base.EncodePackage();
			_buffer.Position = 0;

			byte[] encodedMessage = Buffer;

			byte flags = (byte) Header.Reliability;
			Write((byte) ((flags << 5) | (Header.HasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (encodedMessage.Length*8), true); // length

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