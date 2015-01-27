using System;
using System.Collections.Generic;
using System.IO;

namespace MiNET.Net
{
	public class Datagram: Package
	{
		private int _currentSize = 4; // header
		private MemoryStream _buf;

		public ObjectPool<Datagram> Pool { get; set; }

		public DatagramHeader Header { get; private set; }
		public List<MessagePart> MessageParts { get; set; }


		public Datagram(int sequenceNumber)
		{
			MessageParts = new List<MessagePart>(50);
			Header = new DatagramHeader
			{
				isValid = true,
				needsBAndAs = true,
				datagramSequenceNumber = sequenceNumber
			};
			_buf = new MemoryStream();
		}

		public bool TryAddMessagePart(MessagePart messagePart, int mtuSize)
		{
			var bytes = messagePart.Encode();
			if (bytes.Length + _currentSize > mtuSize) return false;
			if (messagePart.Header.HasSplit && MessageParts.Count > 0) return false;
			//if (Header.isContinuousSend) return false;

			if (messagePart.Header.PartCount > 0 && messagePart.Header.PartIndex > 0) Header.isContinuousSend = true;

			MessageParts.Add(messagePart);
			_currentSize = _currentSize + bytes.Length;

			return true;
		}

		public void Reset()
		{
			Header.datagramSequenceNumber = 0;
			_currentSize = 4;
			MessageParts.Clear();
			MessageParts.Capacity = 50;
			_buf.SetLength(0);
		}

		public byte[] Encode()
		{
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

		public static List<Datagram> CreateDatagrams(List<Package> messages, int mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, ObjectPool<MessagePart> messagePartPool, ObjectPool<Datagram> datagramPool)
		{
			var datagrams = new List<Datagram>(4500);

			Datagram datagram = datagramPool.GetObject();
			datagram.Pool = datagramPool;
			datagram.Header.datagramSequenceNumber = datagramSequenceNumber++;
			datagrams.Add(datagram);
			foreach (var message in messages)
			{
				var messageParts = GetMessageParts(message, mtuSize, ref datagramSequenceNumber, Reliability.RELIABLE, ref reliableMessageNumber, messagePartPool);
				foreach (var messagePart in messageParts)
				{
					if (!datagram.TryAddMessagePart(messagePart, mtuSize))
					{
						datagram = datagramPool.GetObject();
						datagram.Pool = datagramPool;
						datagram.Header.datagramSequenceNumber = datagramSequenceNumber++;
						datagrams.Add(datagram);

						if (!datagram.TryAddMessagePart(messagePart, mtuSize))
						{
							throw new Exception(string.Format("Message part too big for a single datagram. Size: {0}, MTU: {1}", messagePart.Encode().Length, mtuSize));
						}
					}
				}
			}

			return datagrams;
		}

		private static List<MessagePart> GetMessageParts(Package message, int mtuSize, ref int sequenceNumber, Reliability reliability, ref int reliableMessageNumber, ObjectPool<MessagePart> messagePartPool)
		{
			var messageParts = new List<MessagePart>();

			byte[] encodedMessage = message.Encode();
			int count = (int) Math.Ceiling(encodedMessage.Length/((double) mtuSize - 34));
			int index = 0;
			short splitId = (short) (sequenceNumber%short.MaxValue);
			if (encodedMessage.Length <= mtuSize - 34)
			{
				MessagePart messagePart = messagePartPool.GetObject();
				messagePart.MessagePartPool = messagePartPool;
				messagePart.Header.Reliability = reliability;
				messagePart.Header.ReliableMessageNumber = reliableMessageNumber++;
				messagePart.Header.HasSplit = count > 1;
				messagePart.Header.PartCount = count;
				messagePart.Header.PartId = splitId;
				messagePart.Header.PartIndex = index++;
				messagePart.Buffer = encodedMessage;

				messageParts.Add(messagePart);
			}
			else
			{
				foreach (var bytes in ArraySplit(encodedMessage, mtuSize - 34))
				{
					MessagePart messagePart = messagePartPool.GetObject();
					messagePart.MessagePartPool = messagePartPool;
					messagePart.Header.Reliability = reliability;
					messagePart.Header.ReliableMessageNumber = reliableMessageNumber++;
					messagePart.Header.HasSplit = count > 1;
					messagePart.Header.PartCount = count;
					messagePart.Header.PartId = splitId;
					messagePart.Header.PartIndex = index++;
					messagePart.Buffer = bytes;

					messageParts.Add(messagePart);
				}
			}

			return messageParts;
		}

		public static IEnumerable<byte[]> ArraySplit(byte[] bArray, int intBufforLengt)
		{
			int bArrayLenght = bArray.Length;
			byte[] bReturn = null;

			int i = 0;
			for (; bArrayLenght > (i + 1)*intBufforLengt; i++)
			{
				bReturn = new byte[intBufforLengt];
				Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLengt);
				yield return bReturn;
			}

			int intBufforLeft = bArrayLenght - i*intBufforLengt;
			if (intBufforLeft > 0)
			{
				bReturn = new byte[intBufforLeft];
				Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLeft);
				yield return bReturn;
			}
		}
	}
}