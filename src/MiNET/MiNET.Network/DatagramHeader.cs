using System.Collections;
using System.Text;

namespace MiNET.Network
{
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

			//Debug.Print("\t\tPacket data header: {0}", BitsToString(bits));

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
