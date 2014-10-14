using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// friendly name
using MiNET.Network.Utils;

namespace MiNET.Network
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

		public int MessageLength { get; set; }
		public List<Package> Messages { get; set; }
		public byte[] Buffer { get; set; }

		public ConnectedPackage()
		{
			Messages = new List<Package>();
		}

		protected override void EncodePackage()
		{
			_buffer.Position = 0;

			byte[] encodedMessage = Buffer;
			if (Buffer == null)
			{
				encodedMessage = Messages.First().Encode();
			}

			MessageLength = encodedMessage.Length;

			if (_header == null) _header = new DatagramHeader(0x8c);
			if (_splitPacketCount > 1 && _splitPacketIndex > 0)
			{
				Write((byte) 0x8c);
			}
			else
			{
				Write((byte) 0x84);
			}

			Write(_sequenceNumber);

			byte rely = (byte) _reliability;
			Write((byte) ((rely << 5) | (_hasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (MessageLength*8)); // length

			if (_reliability == Reliability.RELIABLE
			    || _reliability == Reliability.RELIABLE_ORDERED
			    || _reliability == Reliability.RELIABLE_SEQUENCED
			    || _reliability == Reliability.RELIABLE_WITH_ACK_RECEIPT
			    || _reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				Write(_reliableMessageNumber);
			}

			if (_reliability == Reliability.UNRELIABLE_SEQUENCED
			    || _reliability == Reliability.RELIABLE_ORDERED
			    || _reliability == Reliability.RELIABLE_SEQUENCED
			    || _reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
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

			Write(encodedMessage);
		}

		protected override void DecodePackage()
		{
			Messages = new List<Package>();

			_buffer.Position = 0;

			_header = new DatagramHeader(ReadByte());
			_sequenceNumber = ReadLittle();

			while (_buffer.Position != _buffer.Length)
			{
				byte flags = ReadByte();


				_reliability = (Reliability) ((flags & Convert.ToByte("011100000", 2)) >> 5);
				int hasSplitPacket = ((flags & Convert.ToByte("00010000", 2)) >> 0);

				short dataBitLength = ReadShort();

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
				MessageLength = (int) Math.Ceiling((((double) dataBitLength)/8));

				byte[] internalBuffer = ReadBytes(MessageLength);
				Messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer));
				if (MessageLength != internalBuffer.Length) Debug.WriteLine("Missmatch of requested lenght, and actual read lenght");
			}
		}
	}

	public enum Reliability
	{
		UNRELIABLE = 0,
		UNRELIABLE_SEQUENCED = 1,
		RELIABLE = 2,
		RELIABLE_ORDERED = 3,
		RELIABLE_SEQUENCED = 4,
		UNRELIABLE_WITH_ACK_RECEIPT = 5,

		RELIABLE_WITH_ACK_RECEIPT = 6,
		RELIABLE_ORDERED_WITH_ACK_RECEIPT = 7
	}
}