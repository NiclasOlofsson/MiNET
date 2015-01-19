// friendly name

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MiNET.Utils;

namespace MiNET.Net
{
	public class Datagram
	{
		private List<MessagePart> _messageParts = new List<MessagePart>();
		private int _currentSize = 4; // header

		public Datagram(int sequenceNumber)
		{
			DatagramHeader = new DatagramHeader
			{
				isValid = true,
				needsBAndAs = true,
				datagramSequenceNumber = sequenceNumber
			};
		}

		public DatagramHeader DatagramHeader { get; private set; }

		public bool AddMessagePart(MessagePart messagePart, int mtuSize)
		{
			if (messagePart.GetPayloadSize() + _currentSize > mtuSize - 60) return false;
			if (messagePart.Header.HasSplit && _messageParts.Count > 0) return false;
			if (DatagramHeader.isContinuousSend) return false;

			if (messagePart.Header.PartCount > 0 && messagePart.Header.PartIndex > 0) DatagramHeader.isContinuousSend = true;

			_messageParts.Add(messagePart);
			_currentSize = _currentSize + messagePart.GetPayloadSize() + 100;

			return true;
		}

		public byte[] Encode()
		{
			MemoryStream buf = new MemoryStream();

			// Header
			buf.WriteByte((byte) (DatagramHeader.isContinuousSend ? 0x8c : 0x84));
			buf.Write(DatagramHeader.datagramSequenceNumber.GetBytes(), 0, 3);

			// Message (Payload)
			foreach (var messagePart in _messageParts)
			{
				byte[] bytes = messagePart.Encode();
				buf.Write(bytes, 0, bytes.Length);
			}

			return buf.ToArray();
		}

		public static List<Datagram> CreateDatagrams(List<Package> messages, int mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber)
		{
			var datagrams = new List<Datagram>();

			Datagram datagram = new Datagram(datagramSequenceNumber++);
			datagrams.Add(datagram);
			foreach (var message in messages)
			{
				var messageParts = GetMessageParts(message, mtuSize, ref datagramSequenceNumber, Reliability.RELIABLE, ref reliableMessageNumber);
				foreach (var messagePart in messageParts)
				{
					if (!datagram.AddMessagePart(messagePart, mtuSize))
					{
						datagrams.Add(datagram);
						datagram = new Datagram(datagramSequenceNumber++);
						datagrams.Add(datagram);

						if (!datagram.AddMessagePart(messagePart, mtuSize))
						{
							throw new Exception("Message part too big for a single datagram");
						}
					}
				}
			}

			return datagrams;
		}

		private static List<MessagePart> GetMessageParts(Package message, int mtuSize, ref int sequenceNumber, Reliability reliability, ref int reliableMessageNumber)
		{
			var messageParts = new List<MessagePart>();

			byte[] encodedMessage = message.Encode();
			int count = (int) Math.Ceiling(encodedMessage.Length/((double) mtuSize - 60));
			int index = 0;
			short splitId = (short) (sequenceNumber%short.MaxValue);
			foreach (var bytes in ArraySplit(encodedMessage, mtuSize - 60))
			{
				MessagePart messagePart = new MessagePart
				{
					Header =
					{
						Reliability = reliability,
						ReliableMessageNumber = reliableMessageNumber++,
						HasSplit = count > 1,
						PartCount = count,
						PartId = splitId,
						PartIndex = index++
					},
					Buffer = bytes
				};

				messageParts.Add(messagePart);
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

		public void SendDatagram(UdpClient socket, IPEndPoint targetEndpoint, byte[] datagram)
		{
			socket.Send(datagram, datagram.Length, targetEndpoint);
		}
	}

	public class MessagePart : Package
	{
		public MessagePartHeader Header { get; private set; }
		public byte[] Buffer { get; set; }

		public MessagePart()
		{
			Header = new MessagePartHeader();
		}

		public int GetPayloadSize()
		{
			return Buffer.Length;
		}

		protected override void EncodePackage()
		{
			_buffer.Position = 0;

			byte[] encodedMessage = Buffer;

			byte flags = (byte) Header.Reliability;
			Write((byte) ((flags << 5) | (Header.HasSplit ? Convert.ToByte("00010000", 2) : 0x00)));
			Write((short) (Buffer.Length*8)); // length

			if (Header.Reliability == Reliability.RELIABLE
			    || Header.Reliability == Reliability.RELIABLE_ORDERED
			    || Header.Reliability == Reliability.RELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_WITH_ACK_RECEIPT
			    || Header.Reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				Write(Header.ReliableMessageNumber);
			}

			if (Header.Reliability == Reliability.UNRELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_ORDERED
			    || Header.Reliability == Reliability.RELIABLE_SEQUENCED
			    || Header.Reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
				)
			{
				Write(Header.OrderingIndex);
				Write(Header.OrderingChannel);
			}

			if (Header.HasSplit)
			{
				Write(Header.PartCount);
				Write(Header.PartId);
				Write(Header.PartIndex);
			}

			// Message body

			Write(encodedMessage);
		}
	}

	public class MessagePartHeader
	{
		public Reliability Reliability { get; set; }
		public Int24 ReliableMessageNumber { get; set; }
		public Int24 SequencingIndex { get; set; }
		public Int24 OrderingIndex { get; set; }
		public byte OrderingChannel { get; set; }

		public bool HasSplit { get; set; }
		public int PartCount { get; set; }
		public short PartId { get; set; }
		public int PartIndex { get; set; }

		public void Decode()
		{
		}

		public void Encode()
		{
		}
	}

	public class ConnectedPackage : Package
	{
		private DatagramHeader _datagramHeader;
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
				Write((byte)0x8c);
			}
			else
			{
				Write((byte)0x84);
			}

			Write(_datagramSequenceNumber);

			// foreach message

			// Message header

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

			// Message body

			Write(encodedMessage);
		}

		protected override void DecodePackage()
		{
			Messages = new List<Package>();

			_buffer.Position = 0;

			_datagramHeader = new DatagramHeader(ReadByte());
			_datagramSequenceNumber = ReadLittle();

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
					_reliableMessageNumber = -1;
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