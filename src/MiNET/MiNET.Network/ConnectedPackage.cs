using System;

namespace MiNET
{
	public class ConnectedPackage : Package
	{
		private DatagramHeader _header;

		public Int24 _sequenceNumber; // uint 24
		public Int24 _reliableMessageNumber;

		public Int24 _sequencingIndex;
		public Int24 _orderingIndex;
		public byte _orderingChannel;
		public int _splitPacketCount;
		public short _splitPacketId;
		public int _splitPacketIndex;
		public Reliability _reliability;
		public bool _hasSplit;

		public byte[] internalBuffer;

		public override void Encode()
		{
			_buffer.Position = 0;

			if (_header == null) _header = new DatagramHeader(0x84);
			Write((byte) 0x84);
			Write(_sequenceNumber);

			Write((byte) (((byte) _reliability << 5) ^ (_hasSplit ? Convert.ToByte("0001", 2) : 0x00)));
			ushort dataBitLenght = (ushort) (internalBuffer.Length*8);
			Write(dataBitLenght); // length

			if (_reliability == Reliability.RELIABLE
				|| _reliability == Reliability.RELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_ORDERED
				)
			{
				Write(_reliableMessageNumber);
			}

			if (_reliability == Reliability.UNRELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_SEQUENCED
				)
			{
				Write(_orderingIndex);
				Write(_orderingChannel);
			}

			if (_hasSplit)
			{
				Write(_splitPacketCount);
				Write(_splitPacketId);
				Write(_splitPacketIndex);
			}

			Write(internalBuffer);
		}

		public override void Decode()
		{
			_buffer.Position = 0;

			_header = new DatagramHeader(ReadByte());
			_sequenceNumber = ReadLittle();

			byte flags = ReadByte();
			_reliability = (Reliability) ((flags & Convert.ToByte("011100000", 2)) >> 5);
			int hasSplitPacket = ((flags & Convert.ToByte("00010000", 2)) >> 0);

			ushort dataBitLength = ReadUShort();

			if (_reliability == Reliability.RELIABLE
				|| _reliability == Reliability.RELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_ORDERED
				)
			{
				_reliableMessageNumber = ReadLittle();
			}
			else
			{
				_reliableMessageNumber = new Int24(-1);
			}

			if (_reliability == Reliability.UNRELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_SEQUENCED
				)
			{
				_sequencingIndex = ReadLittle();
			}

			if (_reliability == Reliability.UNRELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_SEQUENCED
				|| _reliability == Reliability.RELIABLE_ORDERED
				|| _reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				_orderingIndex = ReadLittle();
				_orderingChannel = ReadByte(); // flags
			}
			else
			{
				_orderingChannel = 0;
			}

			if (hasSplitPacket != 0)
			{
				_splitPacketCount = ReadInt();
				_splitPacketId = ReadShort();
				_splitPacketIndex = ReadInt();
			}
			else
			{
				_splitPacketCount = 0;
			}

			// Slurp the payload
			int count = dataBitLength/8;
			internalBuffer = ReadBytes(count);
		}
	}

	public enum Reliability
	{
		UNRELIABLE,
		UNRELIABLE_SEQUENCED,
		RELIABLE,
		RELIABLE_ORDERED,
		RELIABLE_SEQUENCED,
		UNRELIABLE_WITH_ACK_RECEIPT,
		RELIABLE_WITH_ACK_RECEIPT,
		RELIABLE_ORDERED_WITH_ACK_RECEIPT
	}
}
