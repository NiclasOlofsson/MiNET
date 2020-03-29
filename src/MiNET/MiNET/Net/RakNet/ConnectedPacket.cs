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
using log4net;

namespace MiNET.Net.RakNet
{
	public class ConnectedPacket : Packet<ConnectedPacket>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectedPacket));

		public DatagramHeader Header { get; set; }

		public List<Packet> Messages { get; set; } = new List<Packet>();

		public ConnectedPacket()
		{
		}

		protected override void EncodePacket()
		{
			throw new NotImplementedException();

			//_buffer.Position = 0;

			//byte[] encodedMessage = Messages.First().Encode();

			//int messageLength = encodedMessage.Length;

			//// Datagram header

			//if (_datagramHeader == null) _datagramHeader = new DatagramHeader(0x8c);
			//if (_splitPacketCount > 1 && _splitPacketIndex > 0)
			//{
			//	Write((byte) 0x8c);
			//	_hasSplit = true;
			//}
			//else
			//{
			//	_hasSplit = false;
			//	Write((byte) 0x84);
			//}

			//Write(_datagramSequenceNumber);

			//// foreach message

			//// Message header

			//byte rely = (byte) _reliability;
			//Write((byte) ((rely << 5) | (_hasSplit ? 0b00010000 : 0x00)));
			//Write((short) (messageLength * 8), true); // length

			//switch (_reliability)
			//{
			//	case Reliability.Reliable:
			//	case Reliability.ReliableOrdered:
			//	case Reliability.ReliableSequenced:
			//	case Reliability.ReliableWithAckReceipt:
			//	case Reliability.ReliableOrderedWithAckReceipt:
			//		Write(_reliableMessageNumber);
			//		break;
			//}

			//switch (_reliability)
			//{
			//	case Reliability.UnreliableSequenced:
			//	case Reliability.ReliableOrdered:
			//	case Reliability.ReliableSequenced:
			//	case Reliability.ReliableOrderedWithAckReceipt:
			//		Write(_orderingIndex);
			//		Write(_orderingChannel);
			//		break;
			//}

			//if (_hasSplit)
			//{
			//	Write(_splitPacketCount, true);
			//	Write(_splitPacketId, true);
			//	Write(_splitPacketIndex, true);
			//}

			//// Message body

			//Write(encodedMessage);
		}

		protected override void DecodePacket()
		{
			_reader.Position = 0;

			Header = new DatagramHeader(ReadByte());
			Header.DatagramSequenceNumber = ReadLittle();

			// End datagram, online packet starts

			Messages = new List<Packet>();

			while (_reader.Position < _reader.Length)
			{
				byte flags = ReadByte();
				var header = new ReliabilityHeader();

				header.Reliability = (Reliability) ((flags & 0b011100000) >> 5);
				header.HasSplit = (flags & 0b00010000) > 0;

				short dataBitLength = ReadShort(true);

				switch (header.Reliability)
				{
					case Reliability.Reliable:
					case Reliability.ReliableSequenced:
					case Reliability.ReliableOrdered:
						header.ReliableMessageNumber = ReadLittle();
						break;
				}

				switch (header.Reliability)
				{
					case Reliability.UnreliableSequenced:
					case Reliability.ReliableSequenced:
						header.SequencingIndex = ReadLittle();
						break;
				}

				switch (header.Reliability)
				{
					case Reliability.UnreliableSequenced:
					case Reliability.ReliableSequenced:
					case Reliability.ReliableOrdered:
					case Reliability.ReliableOrderedWithAckReceipt:
						header.OrderingIndex = ReadLittle();
						header.OrderingChannel = ReadByte(); // flags
						break;
				}

				if (header.HasSplit)
				{
					header.PartCount = ReadInt(true);
					header.PartId = ReadShort(true);
					header.PartIndex = ReadInt(true);
				}

				// Slurp the payload
				int messageLength = (int) Math.Ceiling((((double) dataBitLength) / 8));
				ReadOnlyMemory<byte> internalBuffer = Slice(messageLength);
				if(internalBuffer.Length != messageLength) Log.Error($"Didn't get expected length {internalBuffer.Length}");
				if(internalBuffer.Length == 0) Log.Error($"Read length {internalBuffer.Length}, expected {messageLength}");
				if(messageLength == 0)  continue;
				if(header.Reliability != Reliability.ReliableOrdered) Log.Error($"Parsing message with reliability={header.Reliability}");

				if (header.HasSplit)
				{
					var splitPartPacket = SplitPartPacket.CreateObject();
					splitPartPacket.ReliabilityHeader = header;
					splitPartPacket.Id = internalBuffer.Span[0];
					splitPartPacket.Message = internalBuffer;
					Messages.Add(splitPartPacket);

					if (Log.IsDebugEnabled && _reader.Position < _reader.Length) Log.Debug($"Got split message, but more to read {_reader.Length - _reader.Position}");
				}
				else
				{
					byte id = internalBuffer.Span[0];
					Packet packet = PacketFactory.Create(id, internalBuffer, "raknet") ?? new UnknownPacket(id, internalBuffer.ToArray());
					packet.ReliabilityHeader = header;

					Messages.Add(packet);
				}

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