using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using little = MiNET.Int24;

namespace MiNET
{
	public struct Int24 : IComparable // later , IConvertible
	{
		private int _value;

		public Int24(byte[] value)
		{
			_value = ToInt24(value).IntValue();
		}

		public Int24(int value)
		{
			_value = value;
		}

		public static Int24 ToInt24(byte[] value)
		{
			if (value.Length > 3) throw new ArgumentOutOfRangeException();
			return new Int24(value[0] | value[1] << 8 | value[2] << 16);
		}

		public byte[] GetBytes()
		{
			return FromInt(_value);
		}

		public int IntValue()
		{
			return _value;
		}

		public static byte[] FromInt(int value)
		{
			byte[] buffer = new byte[3];
			buffer[0] = (byte) value;
			buffer[1] = (byte) (value >> 8);
			buffer[2] = (byte) (value >> 16);
			return buffer;
		}

		public static byte[] FromInt24(Int24 value)
		{
			byte[] buffer = new byte[3];
			buffer[0] = (byte) value.IntValue();
			buffer[1] = (byte) (value.IntValue() >> 8);
			buffer[2] = (byte) (value.IntValue() >> 16);
			return buffer;
		}

		public int CompareTo(object value)
		{
			return _value.CompareTo(value);
		}

		public static explicit operator Int24(byte[] values)
		{
			Int24 d = new Int24(values); // explicit conversion

			return d;
		}

		public static implicit operator Int24(int value)
		{
			Int24 d = new Int24(value); // explicit conversion

			return d;
		}

		public static explicit operator byte[](Int24 d)
		{
			return d.GetBytes();
		}

		public static implicit operator int(Int24 d)
		{
			return d.IntValue(); // implicit conversion
		}
	}

	/// Base package class
	public partial class Package
	{
		public const int MtuSize = 1500;

		public byte Id;

		public MemoryStream _buffer;
		public BinaryWriter _writer;
		public BinaryReader _reader;

		public Package()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public void Write(byte[] value)
		{
			_writer.Write(value);
		}

		public byte[] ReadBytes(int count)
		{
			return _reader.ReadBytes(count);
		}

		public void Write(short value)
		{
			_writer.Write(IPAddress.HostToNetworkOrder(value));
		}

		public short ReadShort()
		{
			return IPAddress.NetworkToHostOrder(_reader.ReadInt16());
		}

		public void Write(ushort value)
		{
			_writer.Write(value);
		}

		public ushort ReadUShort()
		{
			return _reader.ReadUInt16();
		}

		public void Write(little value)
		{
			_writer.Write(value.GetBytes());
		}

		public little ReadLittle()
		{
			return new little(_reader.ReadBytes(3));
		}

		public void Write(int value)
		{
			_writer.Write(value);
		}

		public int ReadInt()
		{
			return _reader.ReadInt32();
		}

		public void Write(uint value)
		{
			_writer.Write(value);
		}

		public uint ReadUInt()
		{
			return _reader.ReadUInt32();
		}

		public void Write(long value)
		{
			_writer.Write(value);
		}

		public long ReadLong()
		{
			return _reader.ReadInt64();
		}

		public void Write(ulong value)
		{
			_writer.Write(value);
		}

		public ulong ReadULong()
		{
			return _reader.ReadUInt64();
		}

		public void Write(float value)
		{
			_writer.Write(value);
		}

		public float ReadFloat()
		{
			return _reader.ReadSingle();
		}

		public void Write(string value)
		{
			_writer.Write(value);
		}

		public string ReadString()
		{
			return _reader.ReadString();
		}

		public virtual void Encode()
		{
			_buffer.Position = 0;
			_writer.Write(Id);
		}

		public virtual void Decode()
		{
			_buffer.Position = 0;
			Id = _reader.ReadByte();
		}
	}

	public class ConnectedPackage : Package
	{
		public byte _header;
		public little _sequenceNumber; // uint 24
		public little _reliableMessageNumber;

		public byte[] _sendBuffer;
		public byte[] _receiveBuffer;
		public Int24 _sequencingIndex;
		public Int24 _orderingIndex;
		public byte _orderingChannel;
		public int _splitPacketCount;
		public short _splitPacketId;
		public int _splitPacketIndex;

		public override void Encode()
		{
			_buffer.Position = 0;
			Write(_header);
			Write(_sequenceNumber);

			//flags & reliability
			Write((byte) 0x40);
			short dataBitLenght = (short) (_sendBuffer.Length*8);
			Write(dataBitLenght); // length
			Write(_reliableMessageNumber); // message number
			Write(_sendBuffer);
		}

		public override void Decode()
		{
			_buffer.Position = 0;
			_header = ReadByte();
			_sequenceNumber = ReadLittle();

			byte flags = ReadByte();
			Reliability reliability = (Reliability) ((flags & Convert.ToByte("011100000", 2)) >> 5);
			int hasSplitPacket = ((flags & Convert.ToByte("00010000", 2)) >> 0);

			ushort dataBitLength = ReadUShort();

			if (reliability == Reliability.RELIABLE
				|| reliability == Reliability.RELIABLE_SEQUENCED
				|| reliability == Reliability.RELIABLE_ORDERED
				)
			{
				_reliableMessageNumber = ReadLittle();
			}
			else
			{
				_reliableMessageNumber = new Int24(-1);
			}

			if (reliability == Reliability.UNRELIABLE_SEQUENCED
				|| reliability == Reliability.RELIABLE_SEQUENCED
				)
			{
				_sequencingIndex = ReadLittle();
			}

			if (reliability == Reliability.UNRELIABLE_SEQUENCED
				|| reliability == Reliability.RELIABLE_SEQUENCED
				|| reliability == Reliability.RELIABLE_ORDERED
				|| reliability == Reliability.RELIABLE_ORDERED_WITH_ACK_RECEIPT
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
			int count = dataBitLength/8;
			_receiveBuffer = ReadBytes(count);
		}

		private enum Reliability
		{
			UNRELIABLE,
			UNRELIABLE_SEQUENCED,
			RELIABLE,
			RELIABLE_ORDERED,
			RELIABLE_SEQUENCED,
			UNRELIABLE_WITH_ACK_RECEIPT,
			RELIABLE_WITH_ACK_RECEIPT,
			RELIABLE_ORDERED_WITH_ACK_RECEIPT
		}
	}

	public class DatagramHeader
	{
		public bool isACK = false;
		public bool isNAK = false;
		public bool isPacketPair = false;
		public bool hasBAndAS = false;
		public bool isContinuousSend = false;
		public bool needsBAndAs = false;
		public bool isValid = false; // To differentiate between what I serialized, and offline data

		public DatagramHeader(byte header)
		{
			var bits = new BitArray(new[] { header });

			Debug.Print("\t\tPacket data header: {0}", BitsToString(bits));

			if (bits[7])
			{
				// IsValid
				isValid = true;
				if (bits[6])
				{
					// IsAck
					isACK = true;
					isPacketPair = false;
					hasBAndAS = bits[5];
					if (hasBAndAS)
					{
						// Read AS
					}
				}
				else if (bits[5])
				{
					// IsNack
					isNAK = true;
					isPacketPair = false;
				}
				else
				{
					// Other
					isACK = false;
					isNAK = false;
					isPacketPair = bits[4];
					isContinuousSend = bits[3];
					needsBAndAs = bits[2];
				}
			}
		}

		public static string BitsToString(BitArray ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			for (int i = 7; 0 <= i; i--)
			{
				hex.AppendFormat("{0},", ba[i]);
			}
			hex.Append("}");
			return hex.ToString();
		}
	}
}
