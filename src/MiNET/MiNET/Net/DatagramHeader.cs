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

using System.Collections;
using System.Text;
using MiNET.Utils;

namespace MiNET.Net
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
		public Int24 datagramSequenceNumber; // uint 24

		public DatagramHeader() : this(0)
		{
		}

		public DatagramHeader(byte header)
		{
			var bits = new BitArray(new[] {header});

			//Debug.Print("\t\tPacket data header: {0}", BitsToString(bits));

			isValid = bits[7];
			isACK = bits[6];
			if (isValid)
			{
				if (isACK)
				{
					isNAK = false;
					isPacketPair = false;
					hasBAndAS = bits[5];
					if (hasBAndAS)
					{
						// Read AS
					}
				}
				else
				{
					isNAK = bits[5];
					if (isNAK)
					{
						// IsNack
						isPacketPair = false;
					}
					else
					{
						// Other
						isPacketPair = bits[4];
						isContinuousSend = bits[3];
						needsBAndAs = bits[2];
					}
				}
			}
		}

		public void Reset()
		{
			isACK = false;
			isNAK = false;
			isPacketPair = false;
			hasBAndAS = false;
			isContinuousSend = false;
			needsBAndAs = false;
			isValid = false;
			datagramSequenceNumber = 0;
		}


		public static string BitsToString(BitArray ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length * 2) + 100);
			hex.Append("{");
			for (int i = 7; 0 <= i; i--)
			{
				hex.AppendFormat("{0}:{1},", i, ba[i]);
			}
			hex.Append("}");
			return hex.ToString();
		}
	}
}