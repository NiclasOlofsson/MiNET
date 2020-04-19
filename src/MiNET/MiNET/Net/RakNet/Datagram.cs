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
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using log4net;

namespace MiNET.Net.RakNet
{
	public class Datagram : Packet<Datagram>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Datagram));

		private int _currentSize = 4; // header
		public long RetransmissionTimeOut { get; set; }
		public bool RetransmitImmediate { get; set; }
		public int TransmissionCount { get; set; }

		public DatagramHeader Header { get; private set; }

		// For encoding
		public List<MessagePart> MessageParts { get; set; }

		// From decoding
		public List<Packet> Messages { get; set; } = new List<Packet>();

		public Datagram()
		{
			MessageParts = new List<MessagePart>(50);
			Header = new DatagramHeader
			{
				IsValid = true,
				NeedsBAndAs = false,
				//datagramSequenceNumber = datagramSequenceNumber
			};
		}

		protected override void DecodePacket()
		{
			_reader.Position = 0;

			Header = new DatagramHeader(ReadByte());
			Header.DatagramSequenceNumber = ReadLittle();

			// End datagram, online packet starts

			Messages = new List<Packet>();

			while (!_reader.Eof)
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
				if (internalBuffer.Length != messageLength) Log.Error($"Didn't get expected length {internalBuffer.Length}");
				if (internalBuffer.Length == 0) Log.Error($"Read length {internalBuffer.Length}, expected {messageLength}");
				if (messageLength == 0) continue;
				//if(header.Reliability != Reliability.ReliableOrdered) Log.Error($"Parsing message {internalBuffer.Span[0]} with reliability={header.Reliability}");

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

		public override byte[] Encode()
		{
			byte[] buffer = ArrayPool<byte>.Shared.Rent(1600);
			ArrayPool<byte>.Shared.Return(buffer);
			using (var buf = new MemoryStream(buffer))
			{
				buf.WriteByte((byte) (Header.IsContinuousSend ? 0x8c : 0x84));
				buf.Write(Header.DatagramSequenceNumber.GetBytes(), 0, 3);

				// Message (Payload)
				foreach (MessagePart messagePart in MessageParts)
				{
					byte[] bytes = messagePart.Encode();
					buf.Write(bytes, 0, bytes.Length);
				}

				return buf.ToArray();
			}
		}

		public long GetEncoded(ref byte[] buffer)
		{
			using var buf = new MemoryStream(buffer);
			// This is a quick-fix to lower the impact of resend. I want to do this
			// as standard, just need to refactor a bit of this stuff first.
			{
				// Header
				//_buf.WriteByte((byte) (Header.IsContinuousSend ? 0x8c : 0x84));
				buf.WriteByte(Header);
				buf.Write(Header.DatagramSequenceNumber.GetBytes(), 0, 3);

				// Message (Payload)
				foreach (MessagePart messagePart in MessageParts)
				{
					byte[] bytes = messagePart.Encode();
					buf.Write(bytes, 0, bytes.Length);
				}
			}

			return buf.Position;
		}

		public override void Reset()
		{
			base.Reset();

			Header.Reset();
			RetransmissionTimeOut = 0;
			RetransmitImmediate = false;
			TransmissionCount = 0;
			_currentSize = 4;
			FirstMessageId = 0;

			foreach (MessagePart part in MessageParts)
			{
				part.PutPool();
			}

			MessageParts.Clear();
		}

		public bool TryAddMessagePart(MessagePart messagePart, int mtuSize)
		{
			byte[] bytes = messagePart.Encode();
			if (_currentSize + bytes.Length > (mtuSize - RakOfflineHandler.UdpHeaderSize)) return false;

			if (messagePart.ReliabilityHeader.PartCount > 0 && messagePart.ReliabilityHeader.PartIndex > 0) Header.IsContinuousSend = true;

			//TODO: Get rid of this stuff.
			if (FirstMessageId == 0) FirstMessageId = messagePart.ContainedMessageId;

			MessageParts.Add(messagePart);

			_currentSize += bytes.Length;

			return true;
		}

		public int FirstMessageId { get; set; }

		public static IEnumerable<Datagram> CreateDatagrams(List<Packet> messages, int mtuSize, RakSession session)
		{
			//Log.Debug($"CreateDatagrams multiple ({messages.Count}) messages");
			Datagram datagram = CreateObject();

			foreach (Packet message in messages)
			{
				List<MessagePart> messageParts = CreateMessageParts(message, mtuSize, session);
				foreach (MessagePart messagePart in messageParts)
				{
					if (!datagram.TryAddMessagePart(messagePart, mtuSize))
					{
						yield return datagram;

						datagram = CreateObject();
						if (datagram.MessageParts.Count != 0) throw new Exception("Excepted no message parts in new message");

						if (!datagram.TryAddMessagePart(messagePart, mtuSize))
						{
							string error = $"Message part too big for a single datagram. Size: {messagePart.Encode().Length}, MTU: {mtuSize}";
							Log.Error(error);
							throw new Exception(error);
						}
					}
				}
			}

			yield return datagram;
		}

		public static IEnumerable<Datagram> CreateDatagrams(Packet message, int mtuSize, RakSession session)
		{
			Log.Warn($"CreateDatagrams single message");
			Datagram datagram = CreateObject();

			List<MessagePart> messageParts = CreateMessageParts(message, mtuSize, session);
			foreach (MessagePart messagePart in messageParts)
			{
				if (!datagram.TryAddMessagePart(messagePart, mtuSize))
				{
					yield return datagram;

					datagram = CreateObject();
					if (datagram.MessageParts.Count != 0) throw new Exception("Excepted no message parts in new message");

					if (!datagram.TryAddMessagePart(messagePart, mtuSize))
					{
						string error = $"Message part too big for a single datagram. Size: {messagePart.Encode().Length}, MTU: {mtuSize}";
						Log.Error(error);
						throw new Exception(error);
					}
				}
			}

			yield return datagram;
		}

		private static List<MessagePart> CreateMessageParts(Packet message, int mtuSize, RakSession session)
		{
			Memory<byte> encodedMessage = message.Encode();

			if (encodedMessage.IsEmpty) return new List<MessagePart>(0);

			if (message.IsMcpe) Log.Error($"Got bedrock message in unexpected place {message.GetType().Name}");

			int maxPayloadSizeNoSplit = mtuSize - RakOfflineHandler.UdpHeaderSize - 4 - GetHeaderSize(message.ReliabilityHeader, false);
			bool split = encodedMessage.Length >= maxPayloadSizeNoSplit;

			List<(int @from, int length)> splits = ArraySplit(encodedMessage.Length, mtuSize - RakOfflineHandler.UdpHeaderSize - 4 /*datagram header*/ - GetHeaderSize(message.ReliabilityHeader, split));
			int count = splits.Count;
			if (count == 0) Log.Warn("Got zero parts back from split");
			if (count <= 1)
			{
				var messagePart = MessagePart.CreateObject();
				messagePart.ReliabilityHeader.Reliability = message.ReliabilityHeader.Reliability;
				messagePart.ReliabilityHeader.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
				messagePart.ReliabilityHeader.OrderingChannel = 0;
				messagePart.ReliabilityHeader.OrderingIndex = message.ReliabilityHeader.OrderingIndex;
				messagePart.ReliabilityHeader.HasSplit = false;
				messagePart.Buffer = encodedMessage;

				return new List<MessagePart>(1) {messagePart};
			}

			// Stupid but scared to change it .. remove the -100 when i feel "safe"
			if (session.SplitPartId > short.MaxValue - 100) Interlocked.CompareExchange(ref session.SplitPartId, 0, short.MaxValue);

			if (message.ReliabilityHeader.Reliability == Reliability.Unreliable) message.ReliabilityHeader.Reliability = Reliability.Reliable;

			int index = 0;
			short splitId = (short) Interlocked.Increment(ref session.SplitPartId);
			var messageParts = new List<MessagePart>(count);
			foreach ((int from, int length) span in splits)
			{
				var messagePart = MessagePart.CreateObject();
				messagePart.ReliabilityHeader.Reliability = message.ReliabilityHeader.Reliability;
				messagePart.ReliabilityHeader.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
				messagePart.ReliabilityHeader.OrderingChannel = 0;
				messagePart.ReliabilityHeader.OrderingIndex = message.ReliabilityHeader.OrderingIndex;
				messagePart.ReliabilityHeader.HasSplit = count > 1;
				messagePart.ReliabilityHeader.PartCount = count;
				messagePart.ReliabilityHeader.PartId = splitId;
				messagePart.ReliabilityHeader.PartIndex = index++;
				messagePart.Buffer = encodedMessage.Slice(span.@from, span.length);

				messageParts.Add(messagePart);
			}

			return messageParts;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int GetHeaderSize(ReliabilityHeader reliabilityHeader, bool split)
		{
			//Write((byte) (flags | (ReliabilityHeader.HasSplit ? 0b00010000 : 0x00)));
			//Write((short) (encodedMessage.Length * 8), true); // bit length
			int size = 3;

			switch (reliabilityHeader.Reliability)
			{
				case Reliability.Reliable:
				case Reliability.ReliableOrdered:
				case Reliability.ReliableSequenced:
				case Reliability.ReliableWithAckReceipt:
				case Reliability.ReliableOrderedWithAckReceipt:
					//Write(reliabilityHeader.ReliableMessageNumber);
					size += 3;
					break;
			}

			//switch (ReliabilityHeader.Reliability)
			//{
			//	case Reliability.UnreliableSequenced:
			//	case Reliability.ReliableSequenced:
			//		ReliabilityHeader.SequencingIndex = WriteLittle();
			//		break;
			//}

			switch (reliabilityHeader.Reliability)
			{
				case Reliability.UnreliableSequenced:
				case Reliability.ReliableOrdered:
				case Reliability.ReliableSequenced:
				case Reliability.ReliableOrderedWithAckReceipt:
					//Write(ReliabilityHeader.OrderingIndex);
					//Write(ReliabilityHeader.OrderingChannel);
					size += 4;
					break;
			}

			if (split)
			{
				//Write(ReliabilityHeader.PartCount, true);
				//Write(ReliabilityHeader.PartId, true);
				//Write(ReliabilityHeader.PartIndex, true);

				size += 10;
			}

			return size;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static List<(int from, int length)> ArraySplit(int length, int intBufferLength)
		{
			List<(int from, int length)> result = new List<(int, int)>();

			int i = 0;
			for (; (i + 1) * intBufferLength < length; i++)
			{
				result.Add((i * intBufferLength, intBufferLength));
			}

			(int form, int length) reminder = (i * intBufferLength, length - i * intBufferLength);
			if (reminder.length > 0)
			{
				result.Add(reminder);
			}

			return result;
		}
	}
}