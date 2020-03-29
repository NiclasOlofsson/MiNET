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
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiNET.Net.RakNet.Tests
{
	[TestClass()]
	public class DatagramTests
	{
		[TestMethod()]
		public void Encode_basic_ok()
		{
			var message = new ConnectionRequestAccepted();
			message.NoBatch = true;
			message.systemAddress = new IPEndPoint(IPAddress.Loopback, 19132);
			message.systemAddresses = new IPEndPoint[20];
			message.systemAddresses[0] = new IPEndPoint(IPAddress.Loopback, 19132);
			message.incomingTimestamp = 12345;
			message.serverTimestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			for (int i = 1; i < 20; i++)
			{
				message.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 19132);
			}


			var datagrams = Datagram.CreateDatagrams(message, 1664, new RakSession(null, null, IPEndPoint.Parse("127.0.0.1"), 1664));
			foreach (Datagram datagram in datagrams)
			{
				var buffer = new byte[1600];
				long length = datagram.GetEncoded(ref buffer);
				Assert.AreNotEqual(length, buffer.Length);
			}
		}
	}
}