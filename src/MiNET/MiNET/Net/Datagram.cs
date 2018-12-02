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
using System.IO;
using System.Threading;
using log4net;
using MiNET.Utils;

namespace MiNET.Net
{
	public class Datagram : Packet<Datagram>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Datagram));

		private int _currentSize = 4; // header
		private MemoryStream _buf;
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
				isValid = true,
				needsBAndAs = true,
				//datagramSequenceNumber = datagramSequenceNumber
			};
			_buf = new MemoryStream(new byte[1600], 0, 1600, true, true);
		}

		public bool TryAddMessagePart(MessagePart messagePart, int mtuSize)
		{
			var bytes = messagePart.Encode();
			if (bytes.Length + _currentSize > mtuSize)
			{
				return false;
			}
			if (messagePart.Header.HasSplit && MessageParts.Count > 0)
			{
				//if (Log.IsDebugEnabled)
				//	Log.Warn($"Message has split and count > 0: {MessageParts.Count}, MTU: {mtuSize}");
				return false;
			}
			//if (Header.isContinuousSend) return false;

			if (messagePart.Header.PartCount > 0 && messagePart.Header.PartIndex > 0) Header.isContinuousSend = true;

			if (FirstMessageId == 0) FirstMessageId = messagePart.ContainedMessageId;

			MessageParts.Add(messagePart);
			_currentSize = _currentSize + bytes.Length;

			return true;
		}

		public int FirstMessageId { get; set; }

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
			_buf.SetLength(0);
		}

		public override byte[] Encode()
		{
			//TODO: This is a qick-fix to lower the impact of resends. I want to do this
			// as standard, just need to refactor a bit of this stuff first.
			if (_buf.Length != 0 && _buf.Length != 1600)
			{
				_buf.Position = 1;
				_buf.Write(Header.datagramSequenceNumber.GetBytes(), 0, 3);

				return _buf.ToArray();
			}

			_buf.SetLength(0);

			// Header
			_buf.WriteByte((byte) (Header.isContinuousSend ? 0x8c : 0x84));
			_buf.Write(Header.datagramSequenceNumber.GetBytes(), 0, 3);

			// Message (Payload)
			foreach (MessagePart messagePart in MessageParts)
			{
				byte[] bytes = messagePart.Encode();
				_buf.Write(bytes, 0, bytes.Length);
			}

			return _buf.ToArray();
		}

		public long GetEncoded(out byte[] buffer)
		{
			//TODO: This is a qick-fix to lower the impact of resends. I want to do this
			// as standard, just need to refactor a bit of this stuff first.
			if (_buf.Length != 0 && _buf.Length != 1600)
			{
				_buf.Position = 1;
				_buf.Write(Header.datagramSequenceNumber.GetBytes(), 0, 3);
			}
			else
			{
				_buf.SetLength(0);

				// Header
				_buf.WriteByte((byte) (Header.isContinuousSend ? 0x8c : 0x84));
				_buf.Write(Header.datagramSequenceNumber.GetBytes(), 0, 3);

				// Message (Payload)
				foreach (MessagePart messagePart in MessageParts)
				{
					byte[] bytes = messagePart.Encode();
					_buf.Write(bytes, 0, bytes.Length);
				}
			}

			buffer = _buf.GetBuffer();
			return _buf.Length;
		}


		public static IEnumerable<Datagram> CreateDatagrams(Packet message, int mtuSize, PlayerNetworkSession session)
		{
			if (message is InternalPing) yield break;

			Datagram datagram = CreateObject();

			List<MessagePart> messageParts = GetMessageParts(message, mtuSize, Reliability.Reliable, session);
			foreach (var messagePart in messageParts)
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

		private static List<MessagePart> GetMessageParts(Packet message, int mtuSize, Reliability reliability, PlayerNetworkSession session)
		{
			var messageParts = new List<MessagePart>();

			Memory<byte> encodedMessage = message.Encode();
			//if (Log.IsDebugEnabled && message is McpeBatch)
			//	Log.Debug($"0x{encodedMessage[0]:x2}\n{Package.HexDump(encodedMessage)}");

			int orderingIndex = 0;

			if (!(message is ConnectedPong) && !(message is DetectLostConnections))
			{
				reliability = Reliability.ReliableOrdered;
			}

			CryptoContext cryptoContext = session.CryptoContext;
			if (cryptoContext != null && !(message is ConnectedPong) && !(message is DetectLostConnections))
			{
				lock (session.EncodeSync)
				{
					reliability = Reliability.ReliableOrdered;

					var isBatch = message is McpeWrapper;

					if (!message.ForceClear && session.CryptoContext.UseEncryption)
					{
						if (isBatch)
						{
							encodedMessage = encodedMessage.Slice(1);
						}
						else
						{
							encodedMessage = Compression.Compress(encodedMessage, true);
						}

						McpeWrapper wrapper = McpeWrapper.CreateObject();
						wrapper.payload = CryptoUtils.Encrypt(encodedMessage, cryptoContext);
						encodedMessage = wrapper.Encode();
						wrapper.PutPool();
					}
					else if (!isBatch)
					{
						McpeWrapper wrapper = McpeWrapper.CreateObject();
						wrapper.payload = Compression.Compress(encodedMessage, true);
						encodedMessage = wrapper.Encode();
						wrapper.PutPool();
					}
					//if (Log.IsDebugEnabled)
					//	Log.Debug($"0x{encodedMessage[0]:x2}\n{Package.HexDump(encodedMessage)}");
				}
			}
			//if (Log.IsDebugEnabled)
			//	Log.Debug($"0x{encodedMessage[0]:x2}\n{Package.HexDump(encodedMessage)}");

			if (reliability == Reliability.ReliableOrdered)
			{
				orderingIndex = Interlocked.Increment(ref session.OrderingIndex);
			}

			if (encodedMessage.IsEmpty) return messageParts;

			int datagramHeaderSize = 100;
			int count = (int) Math.Ceiling(encodedMessage.Length / ((double) mtuSize - datagramHeaderSize));
			int index = 0;
			if (session.SplitPartId > short.MaxValue - 100)
			{
				Interlocked.CompareExchange(ref session.SplitPartId, 0, short.MaxValue);
			}

			short splitId = (short) Interlocked.Increment(ref session.SplitPartId);

			if (count <= 1)
			{
				MessagePart messagePart = MessagePart.CreateObject();
				messagePart.Header.Reliability = reliability;
				messagePart.Header.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
				messagePart.Header.HasSplit = count > 1;
				messagePart.Header.PartCount = count;
				messagePart.Header.PartId = splitId;
				messagePart.Header.PartIndex = index++;
				messagePart.Header.OrderingChannel = 0;
				messagePart.Header.OrderingIndex = orderingIndex;
				messagePart.ContainedMessageId = message.Id;
				messagePart.Buffer = encodedMessage.ToArray();

				messageParts.Add(messagePart);
			}
			else
			{
				foreach ((int from, int to) span in ArraySplit(encodedMessage.Length, mtuSize - datagramHeaderSize))
				{
					MessagePart messagePart = MessagePart.CreateObject();
					messagePart.Header.Reliability = reliability;
					messagePart.Header.ReliableMessageNumber = Interlocked.Increment(ref session.ReliableMessageNumber);
					messagePart.Header.HasSplit = count > 1;
					messagePart.Header.PartCount = count;
					messagePart.Header.PartId = splitId;
					messagePart.Header.PartIndex = index++;
					messagePart.Header.OrderingChannel = 0;
					messagePart.Header.OrderingIndex = orderingIndex;
					messagePart.ContainedMessageId = message.Id;
					messagePart.Buffer = encodedMessage.Slice(span.from, span.to);

					messageParts.Add(messagePart);
				}
			}

			return messageParts;
		}

		public static List<(int from, int to)> ArraySplit(int length, int intBufforLengt)
		{
			List<(int from, int to)> result = new List<(int, int)>();

			int i = 0;
			for (; (i + 1) * intBufforLengt < length; i++)
			{
				result.Add((i * intBufforLengt, intBufforLengt));
			}

			(int, int) reminer = (i * intBufforLengt, length - i * intBufforLengt);
			if (reminer.Item2 > 0)
			{
				result.Add(reminer);
			}

			return result;
		}

		public static IEnumerable<byte[]> ArraySplit(byte[] bArray, int intBufforLengt)
		{
			int bArrayLenght = bArray.Length;
			byte[] bReturn;

			int i = 0;
			for (; bArrayLenght > (i + 1) * intBufforLengt; i++)
			{
				bReturn = new byte[intBufforLengt];
				//Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLengt);
				Buffer.BlockCopy(bArray, i * intBufforLengt, bReturn, 0, intBufforLengt);
				yield return bReturn;
			}

			int intBufforLeft = bArrayLenght - i * intBufforLengt;
			if (intBufforLeft > 0)
			{
				bReturn = new byte[intBufforLeft];
				//Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLeft);
				Buffer.BlockCopy(bArray, i * intBufforLengt, bReturn, 0, intBufforLeft);
				yield return bReturn;
			}
		}
	}
}