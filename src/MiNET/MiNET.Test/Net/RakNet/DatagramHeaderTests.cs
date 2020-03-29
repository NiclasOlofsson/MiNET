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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net.RakNet;

namespace MiNETTests.Net.RakNet
{
	[TestClass()]
	public class DatagramHeaderTests
	{
		[TestMethod()]
		public void DatagramHeader_constructor_continuous_send_ok()
		{
			byte input = 0x8c;
			var header = new DatagramHeader(input);

			Assert.IsTrue(header.IsValid);
			Assert.IsFalse(header.IsAck);
			Assert.IsFalse(header.IsNak);
			Assert.IsFalse(header.IsPacketPair);
			Assert.IsFalse(header.HasBAndAs);
			Assert.IsFalse(header.HasBAndAs);
			Assert.IsTrue(header.IsContinuousSend);
			Assert.IsTrue(header.NeedsBAndAs);
		}

		[TestMethod()]
		public void DatagramHeader_constructor_no_continuous_send_ok()
		{
			byte input = 0x84;
			var header = new DatagramHeader(input);

			Assert.IsTrue(header.IsValid);
			Assert.IsFalse(header.IsAck);
			Assert.IsFalse(header.IsNak);
			Assert.IsFalse(header.IsPacketPair);
			Assert.IsFalse(header.HasBAndAs);
			Assert.IsFalse(header.HasBAndAs);
			Assert.IsFalse(header.IsContinuousSend);
			Assert.IsTrue(header.NeedsBAndAs);
		}

		[TestMethod()]
		public void DatagramHeader_get_byte_continuous_send_ok()
		{
			var header = new DatagramHeader();
			header.IsValid = true;
			header.IsContinuousSend = true;
			header.NeedsBAndAs = true; // remove

			byte result = header;
			Assert.AreEqual(0x8c, result);
		}

		[TestMethod()]
		public void DatagramHeader_get_byte_no_continuous_send_ok()
		{
			var header = new DatagramHeader();
			header.IsValid = true;
			header.NeedsBAndAs = true; // remove

			byte result = header;
			Assert.AreEqual(0x84, result);
		}

		[TestMethod()]
		public void DatagramHeader_get_byte_nak()
		{
			var header = new DatagramHeader();
			header.IsValid = true;
			header.IsNak = true; // remove

			byte result = header;
			Assert.AreEqual(0xa0, result);
		}
	}
}