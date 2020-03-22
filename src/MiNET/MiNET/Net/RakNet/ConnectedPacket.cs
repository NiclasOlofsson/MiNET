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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class ConnectedPacket : Packet<ConnectedPacket>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectedPacket));

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


		public List<Packet> Messages { get; set; } = new List<Packet>();

		public ConnectedPacket()
		{
		}

		protected override void EncodePacket()
		{
			_buffer.Position = 0;

			byte[] encodedMessage = Messages.First().Encode();

			int messageLength = encodedMessage.Length;

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
			Write((byte) ((rely << 5) | (_hasSplit ? 0b00010000 : 0x00)));
			Write((short) (messageLength * 8), true); // length

			switch (_reliability)
			{
				case Reliability.Reliable:
				case Reliability.ReliableOrdered:
				case Reliability.ReliableSequenced:
				case Reliability.ReliableWithAckReceipt:
				case Reliability.ReliableOrderedWithAckReceipt:
					Write(_reliableMessageNumber);
					break;
			}

			switch (_reliability)
			{
				case Reliability.UnreliableSequenced:
				case Reliability.ReliableOrdered:
				case Reliability.ReliableSequenced:
				case Reliability.ReliableOrderedWithAckReceipt:
					Write(_orderingIndex);
					Write(_orderingChannel);
					break;
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

		protected override void DecodePacket()
		{
			Messages = new List<Packet>();

			_reader.Position = 0;

			_datagramHeader = new DatagramHeader(ReadByte());
			_datagramSequenceNumber = ReadLittle();
			_datagramHeader.DatagramSequenceNumber = _datagramSequenceNumber;

			while (_reader.Position < _reader.Length)
			{
				byte flags = ReadByte();
				_reliability = (Reliability) ((flags & 0b011100000) >> 5);
				_hasSplit = ((flags & 0b00010000) > 0);

				short dataBitLength = ReadShort(true);

				switch (_reliability)
				{
					case Reliability.Reliable:
					case Reliability.ReliableSequenced:
					case Reliability.ReliableOrdered:
						_reliableMessageNumber = ReadLittle();
						break;
					default:
						_reliableMessageNumber = -1;
						break;
				}

				switch (_reliability)
				{
					case Reliability.UnreliableSequenced:
					case Reliability.ReliableSequenced:
						_sequencingIndex = ReadLittle();
						break;
					default:
						_sequencingIndex = -1;
						break;
				}

				switch (_reliability)
				{
					case Reliability.UnreliableSequenced:
					case Reliability.ReliableSequenced:
					case Reliability.ReliableOrdered:
					case Reliability.ReliableOrderedWithAckReceipt:
						_orderingIndex = ReadLittle();
						_orderingChannel = ReadByte(); // flags
						break;
					default:
						_orderingIndex = 0;
						_orderingChannel = 0;
						break;
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
				int messageLength = (int) Math.Ceiling((((double) dataBitLength) / 8));
				ReadOnlyMemory<byte> internalBuffer = Slice(messageLength);

				if (_hasSplit)
				{
					var splitPartPacket = SplitPartPacket.CreateObject();
					splitPartPacket.DatagramSequenceNumber = _datagramSequenceNumber;
					splitPartPacket.Reliability = _reliability;
					splitPartPacket.ReliableMessageNumber = _reliableMessageNumber;
					splitPartPacket.OrderingChannel = _orderingChannel;
					splitPartPacket.OrderingIndex = _orderingIndex;
					splitPartPacket.SplitId = _splitPacketId;
					splitPartPacket.SplitCount = _splitPacketCount;
					splitPartPacket.SplitIdx = _splitPacketIndex;
					splitPartPacket.Id = internalBuffer.Span[0];
					splitPartPacket.Message = internalBuffer;
					Messages.Add(splitPartPacket);

					//if (Log.IsDebugEnabled && _buffer.Position < _buffer.Length) Log.Debug($"Got split message, but more to read {_buffer.Length - _buffer.Position}");
					continue;
				}

				byte id = internalBuffer.Span[0];
				Packet packet = PacketFactory.Create(id, internalBuffer, "raknet") ?? new UnknownPacket(id, internalBuffer.ToArray());
				packet.DatagramSequenceNumber = _datagramSequenceNumber;
				packet.Reliability = _reliability;
				packet.ReliableMessageNumber = _reliableMessageNumber;
				packet.OrderingChannel = _orderingChannel;
				packet.OrderingIndex = _orderingIndex;

				//if (!(package is McpeBatch)) Log.Debug($"Raw: {package.DatagramSequenceNumber} {package.ReliableMessageNumber} {package.OrderingIndex} {package.GetType().Name} 0x{package.Id:x2} \n{HexDump(internalBuffer)}");

				Messages.Add(packet);
				if (Log.IsDebugEnabled && messageLength != internalBuffer.Length) Log.Debug("Mismatch of requested length, and actual read length");
			}
		}

		public override void Reset()
		{
			Messages = null;
			base.Reset();
		}
	}
}