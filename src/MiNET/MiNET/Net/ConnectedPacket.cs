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
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Utils;

namespace MiNET.Net
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


		public int MessageLength { get; set; }
		public List<Packet> Messages { get; set; }
		public byte[] Buffer { get; set; }

		public ConnectedPacket()
		{
			Messages = new List<Packet>();
		}

		protected override void EncodePacket()
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
			Write((short) (MessageLength * 8), true); // length

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

		protected override void DecodePacket()
		{
			Messages = new List<Packet>();

			_buffer.Position = 0;

			_datagramHeader = new DatagramHeader(ReadByte());
			_datagramSequenceNumber = ReadLittle();
			_datagramHeader.datagramSequenceNumber = _datagramSequenceNumber;

			_hasSplit = false;
			while (_buffer.Position < _buffer.Length)
			{
				//if (_hasSplit) Log.Warn("Reading second split message");

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
				MessageLength = (int) Math.Ceiling((((double) dataBitLength) / 8));

				byte[] internalBuffer = ReadBytes(MessageLength);

				if (_hasSplit)
				{
					SplitPartPacket splitPartPacket = SplitPartPacket.CreateObject();
					splitPartPacket.DatagramSequenceNumber = _datagramSequenceNumber;
					splitPartPacket.Reliability = _reliability;
					splitPartPacket.ReliableMessageNumber = _reliableMessageNumber;
					splitPartPacket.OrderingChannel = _orderingChannel;
					splitPartPacket.OrderingIndex = _orderingIndex;
					splitPartPacket.SplitId = _splitPacketId;
					splitPartPacket.SplitCount = _splitPacketCount;
					splitPartPacket.SplitIdx = _splitPacketIndex;
					splitPartPacket.Id = internalBuffer[0];
					splitPartPacket.Message = internalBuffer;
					Messages.Add(splitPartPacket);

					//if (Log.IsDebugEnabled && _buffer.Position < _buffer.Length) Log.Debug($"Got split message, but more to read {_buffer.Length - _buffer.Position}");
					continue;
				}

				byte id = internalBuffer[0];
				Packet packet = PacketFactory.Create(id, internalBuffer, "raknet") ?? new UnknownPacket(id, internalBuffer);
				packet.DatagramSequenceNumber = _datagramSequenceNumber;
				packet.Reliability = _reliability;
				packet.ReliableMessageNumber = _reliableMessageNumber;
				packet.OrderingChannel = _orderingChannel;
				packet.OrderingIndex = _orderingIndex;

				//if (!(package is McpeBatch)) Log.Debug($"Raw: {package.DatagramSequenceNumber} {package.ReliableMessageNumber} {package.OrderingIndex} {package.GetType().Name} 0x{package.Id:x2} \n{HexDump(internalBuffer)}");

				Messages.Add(packet);
				if (Log.IsDebugEnabled && MessageLength != internalBuffer.Length) Log.Debug("Mismatch of requested length, and actual read length");
			}
		}

		public override void Reset()
		{
			Messages = null;
			base.Reset();
		}
	}
}