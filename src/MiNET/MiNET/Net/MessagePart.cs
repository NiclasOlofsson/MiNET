using System;

namespace MiNET.Net
{
	public class MessagePart : Package<MessagePart> // Replace this with stream
	{
		public MessagePartHeader Header { get; private set; }
		public byte[] Buffer { get; set; }

		public MessagePart()
		{
			Header = new MessagePartHeader();
		}

		public int GetPayloadSize()
		{
			return Buffer.Length;
		}

		protected override void EncodePackage()
		{
			// DO NOT CALL base.EncodePackage();
			_buffer.Position = 0;

			byte[] encodedMessage = Buffer;

			byte flags = (byte) Header.Reliability;
			Write((byte) ((flags << 5) | (Header.HasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (encodedMessage.Length*8)); // length

			if (Header.Reliability == Reliability.RELIABLE
			    || Header.Reliability == Reliability.RELIABLE_ORDERED
			    || Header.Reliability == Reliability.RELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_WITH_ACK_RECEIPT
			    || Header.Reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				Write(Header.ReliableMessageNumber);
			}

			if (Header.Reliability == Reliability.UNRELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_ORDERED
			    || Header.Reliability == Reliability.RELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				Write(Header.OrderingIndex);
				Write(Header.OrderingChannel);
			}

			if (Header.HasSplit)
			{
				Write(Header.PartCount);
				Write(Header.PartId);
				Write(Header.PartIndex);
			}

			// Message body

			Write(encodedMessage);
		}
	}
}