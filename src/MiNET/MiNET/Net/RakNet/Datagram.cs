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
using System.IO.Compression;
using System.Threading;
using log4net;
using MiNET.Utils;

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
		public List<MessagePart> MessageParts { get; set; }

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

		public bool TryAddMessagePart(MessagePart messagePart, int mtuSize)
		{
			byte[] bytes = messagePart.Encode();
			if (bytes.Length + _currentSize > mtuSize) return false;

			if (messagePart.ReliabilityHeader.HasSplit && MessageParts.Count > 0)
			{
				//if (Log.IsDebugEnabled)
				//	Log.Warn($"Message has split and count > 0: {MessageParts.Count}, MTU: {mtuSize}");
				return false;
			}
			//if (Header.isContinuousSend) return false;

			if (messagePart.ReliabilityHeader.PartCount > 0 && messagePart.ReliabilityHeader.PartIndex > 0) Header.IsContinuousSend = true;

			if (FirstMessageId == 0) FirstMessageId = messagePart.ContainedMessageId;

			MessageParts.Add(messagePart);
			_currentSize = _currentSize + bytes.Length;

			return true;
		}

		public int FirstMessageId { get; set; }

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
			using (var buf = new MemoryStream(buffer))
			{
				// This is a quick-fix to lower the impact of resend. I want to do this
				// as standard, just need to refactor a bit of this stuff first.
				{
					if (MessageParts.Count > 1) Log.Error($"Got {MessageParts.Count} message parts");

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

		public static IEnumerable<Datagram> CreateDatagrams(Packet message, int mtuSize, RakSession session)
		{
			Datagram datagram = CreateObject();

			List<MessagePart> messageParts = CreateMessageParts(message, mtuSize, Reliability.Reliable, session);
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

		private static List<MessagePart> CreateMessageParts(Packet message, int mtuSize, Reliability reliability, RakSession session)
		{
			Memory<byte> encodedMessage = message.Encode();

			if (encodedMessage.IsEmpty) return new List<MessagePart>(0);

			// All MCPE messages goes into a compressed (and possible encrypted) wrapper.
			// Note that McpeWrapper itself is a RakNet level message.
			bool isWrapper = message is McpeWrapper;
			if (message.IsMcpe)
			{
				var wrapper = McpeWrapper.CreateObject();
				wrapper.payload = Compression.Compress(encodedMessage, true, encodedMessage.Length > 1000 ? CompressionLevel.Fastest : CompressionLevel.NoCompression);
				encodedMessage = wrapper.Encode();
				wrapper.PutPool();
				isWrapper = true;
			}

			// Should probably only force for McpeWrapper, not the other messages (RakNet)
			if (!(message is ConnectedPong) && !(message is DetectLostConnections)) reliability = Reliability.ReliableOrdered;

			int orderingIndex = 0;
			lock (session.EncodeSync)
			{
				CryptoContext cryptoContext = session.CryptoContext;
				if (!message.ForceClear && cryptoContext != null && session.CryptoContext.UseEncryption && isWrapper)
				{
					var wrapper = McpeWrapper.CreateObject();
					wrapper.payload = CryptoUtils.Encrypt(encodedMessage.Slice(1), cryptoContext);
					encodedMessage = wrapper.Encode();
					wrapper.PutPool();
				}

				if (reliability == Reliability.ReliableOrdered) orderingIndex = Interlocked.Increment(ref session.OrderingIndex);
			}

			List<(int @from, int length)> splits = ArraySplit(encodedMessage.Length, mtuSize - 100);
			int count = splits.Count;
			if (count == 0) Log.Warn("Got zero parts back from split");
			if (count <= 1)
			{
				var messagePart = MessagePart.CreateObject();
				messagePart.ReliabilityHeader.Reliability = reliability;
				messagePart.ReliabilityHeader.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
				messagePart.ReliabilityHeader.OrderingChannel = 0;
				messagePart.ReliabilityHeader.OrderingIndex = orderingIndex;
				messagePart.ReliabilityHeader.HasSplit = false;
				messagePart.Buffer = encodedMessage;

				return new List<MessagePart>(1) {messagePart};
			}

			// Stupid but scared to change it .. remove the -100 when i feel "safe"
			if (session.SplitPartId > short.MaxValue - 100) Interlocked.CompareExchange(ref session.SplitPartId, 0, short.MaxValue);

			int index = 0;
			short splitId = (short) Interlocked.Increment(ref session.SplitPartId);
			var messageParts = new List<MessagePart>(count);
			foreach ((int from, int length) span in splits)
			{
				var messagePart = MessagePart.CreateObject();
				messagePart.ReliabilityHeader.Reliability = reliability;
				messagePart.ReliabilityHeader.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
				messagePart.ReliabilityHeader.OrderingChannel = 0;
				messagePart.ReliabilityHeader.OrderingIndex = orderingIndex;
				messagePart.ReliabilityHeader.HasSplit = count > 1;
				messagePart.ReliabilityHeader.PartCount = count;
				messagePart.ReliabilityHeader.PartId = splitId;
				messagePart.ReliabilityHeader.PartIndex = index++;
				messagePart.Buffer = encodedMessage.Slice(span.@from, span.length);

				messageParts.Add(messagePart);
			}

			return messageParts;
		}

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