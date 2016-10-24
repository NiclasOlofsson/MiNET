// friendly name

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using MiNET.Utils;

namespace MiNET.Net
{
	public class ConnectedPackage : Package<ConnectedPackage>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ConnectedPackage));

		public DatagramHeader _datagramHeader;
		public Int24 _datagramSequenceNumber; // uint 24

		public Reliability _reliability;
		public Int24 _reliableMessageNumber;

		public Int24 _sequencingIndex;
		public Int24 _orderingIndex;
		public byte _orderingChannel;

		public bool _hasSplit;
		public int _splitPacketCount;
		public short _splitPacketId;
		public int _splitPacketIndex;


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

			// Datagram header

			if (_datagramHeader == null) _datagramHeader = new DatagramHeader(0x8c);
			if (_splitPacketCount > 1 && _splitPacketIndex > 0)
			{
				Write((byte) 0x8c);
				_hasSplit = true;
			}
			else
			{
				_hasSplit = false;
				Write((byte) 0x84);
			}

			Write(_datagramSequenceNumber);

			// foreach message

			// Message header

			byte rely = (byte) _reliability;
			Write((byte) ((rely << 5) | (_hasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (MessageLength*8), true); // length

			if (_reliability == Reliability.Reliable
			    || _reliability == Reliability.ReliableOrdered
			    || _reliability == Reliability.ReliableSequenced
			    || _reliability == Reliability.ReliableWithAckReceipt
			    || _reliability == Reliability.ReliableOrderedWithAckReceipt
				)
			{
				Write(_reliableMessageNumber);
			}

			if (_reliability == Reliability.UnreliableSequenced
			    || _reliability == Reliability.ReliableOrdered
			    || _reliability == Reliability.ReliableSequenced
			    || _reliability == Reliability.ReliableOrderedWithAckReceipt
				)
			{
				Write(_orderingIndex);
				Write(_orderingChannel);
			}

			if (_hasSplit)
			{
				Write(_splitPacketCount, true);
				Write(_splitPacketId, true);
				Write(_splitPacketIndex, true);
			}

			// Message body

			Write(encodedMessage);
		}

		protected override void DecodePackage()
		{
			Messages = new List<Package>();

			_buffer.Position = 0;

			_datagramHeader = new DatagramHeader(ReadByte());
			_datagramSequenceNumber = ReadLittle();
			_datagramHeader.datagramSequenceNumber = _datagramSequenceNumber;

			_hasSplit = false;
			while (_buffer.Position < _buffer.Length)
			{
				if(_hasSplit) Log.Warn("Reading second split message");

				byte flags = ReadByte();
				_reliability = (Reliability) ((flags & Convert.ToByte("011100000", 2)) >> 5);
				_hasSplit = ((flags & Convert.ToByte("00010000", 2)) > 0);

				short dataBitLength = ReadShort(true);

				if (_reliability == Reliability.Reliable
				    || _reliability == Reliability.ReliableSequenced
				    || _reliability == Reliability.ReliableOrdered
					)
				{
					_reliableMessageNumber = ReadLittle();
				}
				else
				{
					_reliableMessageNumber = -1;
				}

				if (_reliability == Reliability.UnreliableSequenced
				    || _reliability == Reliability.ReliableSequenced
					)
				{
					_sequencingIndex = ReadLittle();
				}
				else
				{
					_sequencingIndex = -1;
				}

				if (_reliability == Reliability.UnreliableSequenced
				    || _reliability == Reliability.ReliableSequenced
				    || _reliability == Reliability.ReliableOrdered
				    || _reliability == Reliability.ReliableOrderedWithAckReceipt
					)
				{
					_orderingIndex = ReadLittle();
					_orderingChannel = ReadByte(); // flags
				}
				else
				{
					_orderingIndex = 0;
					_orderingChannel = 0;
				}

				if (_hasSplit)
				{
					_splitPacketCount = ReadInt(true);
					_splitPacketId = ReadShort(true);
					_splitPacketIndex = ReadInt(true);
				}
				else
				{
					_splitPacketCount = -1;
					_splitPacketId = -1;
					_splitPacketIndex = -1;
				}

				// Slurp the payload
				MessageLength = (int) Math.Ceiling((((double) dataBitLength)/8));

				byte[] internalBuffer = ReadBytes(MessageLength);

				if (_hasSplit)
				{
					SplitPartPackage splitPartPackage = SplitPartPackage.CreateObject();
					splitPartPackage.DatagramSequenceNumber = _datagramSequenceNumber;
					splitPartPackage.Reliability = _reliability;
					splitPartPackage.ReliableMessageNumber = _reliableMessageNumber;
					splitPartPackage.OrderingChannel = _orderingChannel;
					splitPartPackage.OrderingIndex = _orderingIndex;
					splitPartPackage.SplitId = _splitPacketId;
					splitPartPackage.SplitCount = _splitPacketCount;
					splitPartPackage.SplitIdx = _splitPacketIndex;
					splitPartPackage.Id = internalBuffer[0];
					splitPartPackage.Message = internalBuffer;
					Messages.Add(splitPartPackage);

					if (Log.IsDebugEnabled && _buffer.Position < _buffer.Length) Log.Debug($"Got split message, but more to read {_buffer.Length - _buffer.Position}");
					continue;
				}

				byte id = internalBuffer[0];
				Package package = PackageFactory.CreatePackage(id, internalBuffer, "raknet") ?? new UnknownPackage(id, internalBuffer);
				package.DatagramSequenceNumber = _datagramSequenceNumber;
				package.Reliability = _reliability;
				package.ReliableMessageNumber = _reliableMessageNumber;
				package.OrderingChannel = _orderingChannel;
				package.OrderingIndex = _orderingIndex;

				//if (!(package is McpeBatch)) Log.Debug($"Raw: {package.DatagramSequenceNumber} {package.ReliableMessageNumber} {package.OrderingIndex} {package.GetType().Name} 0x{package.Id:x2} \n{HexDump(internalBuffer)}");

				Messages.Add(package);
				if (Log.IsDebugEnabled && MessageLength != internalBuffer.Length) Log.Debug("Missmatch of requested lenght, and actual read lenght");
			}
		}

		public override void Reset()
		{
			Messages = null;
			base.Reset();
		}
	}
}