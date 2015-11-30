using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using log4net;

namespace MiNET.Net
{
	public class Datagram : Package<Datagram>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Datagram));

		private int _currentSize = 4; // header
		private MemoryStream _buf;
		public long RetransmissionTimeOut { get; set; }
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
			_buf = new MemoryStream();
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
				//Log.Warn(string.Format("Message has split and count > 0: {0}, MTU: {1}", MessageParts.Count, mtuSize));
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
			//base.Reset();

			Header.Reset();
			RetransmissionTimeOut = 0;
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

		public static IEnumerable<Datagram> CreateDatagrams(Package message, int mtuSize, PlayerNetworkSession session)
		{
			if (message is InternalPing) yield break;

			Datagram datagram = CreateObject();
			//datagram.Reset();

			var messageParts = GetMessageParts(message, mtuSize, Reliability.Reliable, ref session.ReliableMessageNumber);
			foreach (var messagePart in messageParts)
			{
				if (!datagram.TryAddMessagePart(messagePart, mtuSize))
				{
					yield return datagram;

					datagram = CreateObject();
					//datagram.Reset();

					if (!datagram.TryAddMessagePart(messagePart, mtuSize))
					{
						Log.Warn(string.Format("Message part too big for a single datagram. Size: {0}, MTU: {1}", messagePart.Encode().Length, mtuSize));
						throw new Exception(string.Format("Message part too big for a single datagram. Size: {0}, MTU: {1}", messagePart.Encode().Length, mtuSize));
					}
				}
			}

			yield return datagram;
		}

		private static List<MessagePart> GetMessageParts(Package message, int mtuSize, Reliability reliability, ref int reliableMessageNumber)
		{
			var messageParts = new List<MessagePart>();

			byte[] encodedMessage = message.Encode();
			if (encodedMessage == null) return messageParts;

			int datagramHeaderSize = 60;
			//int datagramHeaderSize = 100;
			int count = (int) Math.Ceiling(encodedMessage.Length/((double) mtuSize - datagramHeaderSize));
			int index = 0;
			short splitId = (short) (DateTime.UtcNow.Ticks%short.MaxValue);
			if (count <= 1)
			{
				MessagePart messagePart = MessagePart.CreateObject();
				messagePart.Header.Reliability = reliability;
				messagePart.Header.ReliableMessageNumber = Interlocked.Increment(ref reliableMessageNumber);
				messagePart.Header.HasSplit = count > 1;
				messagePart.Header.PartCount = count;
				messagePart.Header.PartId = splitId;
				messagePart.Header.PartIndex = index++;
				messagePart.ContainedMessageId = message.Id;
				messagePart.Buffer = encodedMessage;

				messageParts.Add(messagePart);
			}
			else
			{
				foreach (var bytes in ArraySplit(encodedMessage, mtuSize - datagramHeaderSize))
				{
					MessagePart messagePart = MessagePart.CreateObject();
					messagePart.Header.Reliability = reliability;
					messagePart.Header.ReliableMessageNumber = Interlocked.Increment(ref reliableMessageNumber);
					messagePart.Header.HasSplit = count > 1;
					messagePart.Header.PartCount = count;
					messagePart.Header.PartId = splitId;
					messagePart.Header.PartIndex = index++;
					messagePart.ContainedMessageId = message.Id;
					messagePart.Buffer = bytes;

					messageParts.Add(messagePart);
				}
			}

			return messageParts;
		}

		public static IEnumerable<byte[]> ArraySplit(byte[] bArray, int intBufforLengt)
		{
			int bArrayLenght = bArray.Length;
			byte[] bReturn;

			int i = 0;
			for (; bArrayLenght > (i + 1)*intBufforLengt; i++)
			{
				bReturn = new byte[intBufforLengt];
				//Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLengt);
				Buffer.BlockCopy(bArray, i*intBufforLengt, bReturn, 0, intBufforLengt);
				yield return bReturn;
			}

			int intBufforLeft = bArrayLenght - i*intBufforLengt;
			if (intBufforLeft > 0)
			{
				bReturn = new byte[intBufforLeft];
				//Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLeft);
				Buffer.BlockCopy(bArray, i*intBufforLengt, bReturn, 0, intBufforLeft);
				yield return bReturn;
			}
		}
	}
}