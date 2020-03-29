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

using System.Collections;
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class DatagramHeader
	{
		public bool IsAck { get; set; }
		public bool IsNak { get; set; }
		public bool IsPacketPair { get; set; }
		public bool HasBAndAs { get; set; }
		public bool IsContinuousSend { get; set; }
		public bool NeedsBAndAs { get; set; }
		public bool IsValid { get; set; }

		public Int24 DatagramSequenceNumber; // uint 24

		public DatagramHeader() : this(0)
		{
		}

		public DatagramHeader(byte header)
		{
			var bits = new BitArray(new[] {header});

			IsValid = bits[7];
			IsAck = bits[6];
			if (IsValid)
			{
				if (IsAck)
				{
					IsNak = false;
					IsPacketPair = false;
					HasBAndAs = bits[5];
					if (HasBAndAs)
					{
						// Read AS
					}
				}
				else
				{
					IsNak = bits[5];
					if (IsNak)
					{
						// IsNack
						IsPacketPair = false;
					}
					else
					{
						// Other
						IsPacketPair = bits[4];
						IsContinuousSend = bits[3];
						NeedsBAndAs = bits[2];
					}
				}
			}
		}

		public void Reset()
		{
			IsAck = false;
			IsNak = false;
			IsPacketPair = false;
			HasBAndAs = false;
			IsContinuousSend = false;
			NeedsBAndAs = false;
			IsValid = false;
			DatagramSequenceNumber = 0;
		}

		public static implicit operator byte(DatagramHeader h)
		{
			var bits = new BitArray(8);

			if (h.IsValid)
			{
				bits[7] = h.IsValid;

				if (h.IsAck)
				{
					bits[6] = h.IsAck;
					bits[5] = h.HasBAndAs;
				}
				else
				{
					bits[5] = h.IsNak;
					if (h.IsNak)
					{
					}
					else
					{
						// Other
						bits[4] = h.IsPacketPair;
						bits[3] = h.IsContinuousSend;
						bits[2] = h.NeedsBAndAs;
					}
				}
			}

			var bytes = new byte[1];
			bits.CopyTo(bytes, 0);

			return bytes[0];
		}
	}
}